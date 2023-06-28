using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Model.Graph.Editor
{
    [EditorTool("CreateGraph")]
    public class GraphTool : EditorTool
    {
        private Edge edgePrefab;
        private Node nodePrefab;
        private GameObject graph;


        private Type[] graphTypes = new[]
        {
            typeof(Node),
            typeof(Edge)
        };
        
        public override void OnActivated()
        {
            base.OnActivated();

            edgePrefab = Resources.Load<Edge>("Prefabs/Line");
            nodePrefab = Resources.Load<Node>("Prefabs/Node");
            graph = GameObject.Find("Graph");
            
            foreach (var gameObject in FindObjectsOfType<GameObject>())
                SceneVisibilityManager.instance.DisablePicking(gameObject,false);
            
            foreach (var node in FindObjectsOfType<Node>())
                SceneVisibilityManager.instance.EnablePicking(node.gameObject,false);
            
            SceneVisibilityManager.instance.EnablePicking(graph,false);
            EditorApplication.RepaintHierarchyWindow();
            
            Debug.Log("CreateGraph is Activated!");
        }

        public override void OnWillBeDeactivated()
        {
            base.OnWillBeDeactivated();
            foreach (var gameObject in FindObjectsOfType<GameObject>())
                SceneVisibilityManager.instance.EnablePicking(gameObject, false);
            
            EditorApplication.RepaintHierarchyWindow();
            Debug.Log("CreateGraph is Deactivated!");
        }

        public override void OnToolGUI(EditorWindow window)
        {
            UsePositionHandle();
            if (Event.current.Equals(Event.KeyboardEvent("k")))
            {
                PutNodeInMousePosition();
            }
        }

        private void PutNodeInMousePosition()
        {
            var activeNodes = Selection.gameObjects
                .Select(o => o.GetComponent<Node>())
                .Where(o => o != null)
                .ToArray();
            switch (activeNodes.Length)
            {
                case 0:
                    CreateNode();
                    break;
                case 1:
                    var newNode = CreateNode();
                    ConnectNodes(newNode, activeNodes[0]);
                    break;
                case 2:
                    ConnectOrDisconnectNodes(activeNodes[0], activeNodes[1]);
                    break;
                case > 2:
                    Debug.Log("Too many nodes");
                    break;
            }
        }

        private void ConnectOrDisconnectNodes(Node firstNode, Node secondNode)
        {
            var intersect = GetIntersectEdges(firstNode, secondNode);
            if (intersect.Count == 0)
                ConnectNodes(firstNode, secondNode);
            else
                DisconnectNodes(firstNode, secondNode, intersect);
        }

        private Edge ConnectNodes(Node firstNode, Node secondNode)
        {
            Undo.IncrementCurrentGroup();
            
            var edge = CreateEdge();
            edge.Initialise(firstNode, secondNode);
            
            
            Undo.RecordObject(firstNode, "ConnectNodes");
            firstNode.AddEdge(edge);
            Undo.RecordObject(secondNode, "ConnectNodes");
            secondNode.AddEdge(edge);
            
            EditorUtility.SetDirty(firstNode);
            EditorUtility.SetDirty(secondNode);
            EditorUtility.SetDirty(edge);
            
            return edge;
        }

        private Edge CreateEdge()
        {
            var edge = Instantiate(edgePrefab, graph.transform);
            Undo.RegisterCreatedObjectUndo(edge.gameObject, "CreateEdge");
            SceneVisibilityManager.instance.DisablePicking(edge.gameObject, false);
            return edge;
        }
        
        private void DisconnectNodes(Node firstNode, Node secondNode)
        {
            var intersect = GetIntersectEdges(firstNode, secondNode);
            DisconnectNodes(firstNode, secondNode, intersect);
        }

        private void DisconnectNodes(Node firstNode, Node secondNode, List<Edge> intersect)
        {
            Undo.IncrementCurrentGroup();
            Undo.RecordObject(firstNode, "DisconnectNodes");
            Undo.RecordObject(secondNode, "DisconnectNodes");
            
            foreach (var edge in intersect)
            {
                firstNode.RemoveEdge(edge);
                secondNode.RemoveEdge(edge);
                Undo.DestroyObjectImmediate(edge.gameObject);
            }
            
            EditorUtility.SetDirty(firstNode);
            EditorUtility.SetDirty(secondNode);
        }

        private Node CreateNode()
        {
            Undo.IncrementCurrentGroup();
            
            var node = Instantiate(nodePrefab, GetMousePosition2D(), Quaternion.identity, graph.transform);
            node.Initialise();
            Selection.activeObject = node.gameObject;

            Undo.RegisterCreatedObjectUndo(node.gameObject, "CreateNode");
            EditorUtility.SetDirty(node);
            return node;
        }

        public void DeleteNode(Node node)
        {
            node.BeforeDestroy(out var deletedEdges, out var neighbors);
            DestroyImmediate(node.gameObject);
            foreach (var edge in deletedEdges)
                DestroyImmediate(edge.gameObject);
            foreach (var neighbor in neighbors)
                EditorUtility.SetDirty(neighbor);
        }
        
        private void UsePositionHandle()
        {
            if (target is GameObject activeObj)
            {
                if (target.GetComponent<Node>() == null) return;
                
                EditorGUI.BeginChangeCheck();
                var oldPos = activeObj.transform.position;
                var newPos = Handles.PositionHandle(oldPos, Quaternion.identity);
                var offset = newPos - oldPos;
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (var node in targets
                                 .Select(o => (GameObject) o)
                                 .Select(o => o.GetComponent<Node>())
                                 .Where(o => o != null)
                            )
                    {
                        Undo.RecordObject(node.transform, "Move Node");
                        node.transform.position += offset;
                        ReDrawEdges(node);
                    }
                }
            }
        }

        private void ReDrawEdges(Node node)
        {
            foreach (var edge in node.GetEdgesList())
            {
                Undo.RecordObject(edge.transform, "ReDrawEdge");
                Undo.RecordObject(edge.Drawer.LineSpriteRenderer, "ReDrawEdge");
                edge.ReDraw();
            }
        }
        
        

        private bool CheckGameObjectForExistAnyComponents(GameObject o, params Type[] components)
        {
            foreach (var component in components)
            {
                if (o.GetComponent(component) != null)
                    return true;
            }

            return false;
        }

        private List<Edge> GetIntersectEdges(Node firstNode, Node secondNode)
        {
            return firstNode.GetEdgesList()
                .Intersect(secondNode.GetEdgesList())
                .ToList();
        }

        private Vector2 GetMousePosition2D()
        {
            var mousePos = Event.current.mousePosition;
            var mouseX = mousePos.x;
            var mouseY = Camera.current.pixelHeight - mousePos.y;
            var coord = Camera.current.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0));
            return coord;
        }
    }
}