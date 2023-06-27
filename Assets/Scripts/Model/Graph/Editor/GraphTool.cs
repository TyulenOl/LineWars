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
            Debug.Log("CreateGraph is Activated!");
            
            edgePrefab = Resources.Load<Edge>("Prefabs/Line");
            nodePrefab = Resources.Load<Node>("Prefabs/Node");
            graph = GameObject.Find("Graph");
        }

        public override void OnWillBeDeactivated()
        {
            base.OnWillBeDeactivated();
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
            var edge = Instantiate(edgePrefab);
            edge.Initialise(firstNode, secondNode);
            firstNode.AddEdge(edge);
            secondNode.AddEdge(edge);
            
            EditorUtility.SetDirty(edge);
            EditorUtility.SetDirty(firstNode);
            EditorUtility.SetDirty(secondNode);
            
            return edge;
        }

        private void DisconnectNodes(Node firstNode, Node secondNode)
        {
            var intersect = GetIntersectEdges(firstNode, secondNode);
            DisconnectNodes(firstNode, secondNode, intersect);
        }

        private void DisconnectNodes(Node firstNode, Node secondNode, List<Edge> intersect)
        {
            foreach (var edge in intersect)
            {
                firstNode.RemoveEdge(edge);
                secondNode.RemoveEdge(edge);
                DestroyImmediate(edge);
            }
            
            EditorUtility.SetDirty(firstNode);
            EditorUtility.SetDirty(secondNode);
        }

        private Node CreateNode()
        {
            var node = Instantiate(nodePrefab, GetMousePosition(), Quaternion.identity, graph.transform);
            node.Initialise();
            Selection.activeObject = node;
            
            EditorUtility.SetDirty(node);
            return node;
        }

        public void DeleteNode(Node node)
        {
            node.BeforeDestroy(out var deletedEdges, out var neighbors);
            DestroyImmediate(node);
            foreach (var edge in deletedEdges)
                DestroyImmediate(edge);
            foreach (var neighbor in neighbors)
                EditorUtility.SetDirty(neighbor);
        }
        
        private void UsePositionHandle()
        {
            if (target is GameObject activeObj)
            {
                if (target.GetComponent<Node>() == null)
                    return;
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
                edge.ReDraw();
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

        private Vector3 GetMousePosition()
        {
            var mousePos = Event.current.mousePosition;
            var mouseX = mousePos.x;
            var mouseY = Camera.current.pixelHeight - mousePos.y;
            var myRay = Camera.current.ScreenPointToRay(new Vector3(mouseX, mouseY, 0));
            return Physics.Raycast(myRay, out var hitInfo) ? hitInfo.point : default(Vector2);
        }
    }
}