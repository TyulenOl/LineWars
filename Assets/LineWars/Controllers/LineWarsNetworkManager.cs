using System;
using System.Collections.Generic;
using Mirror;
using Model.Graph;
using UnityEngine;

namespace Controllers
{
    public class LineWarsNetworkManager: NetworkManager
    {
        public static LineWarsNetworkManager Instance => (LineWarsNetworkManager) singleton;
        
        [Header("LineWars")]
        [SerializeField] private int levelNumber; // cons

        private Node nodePrefab;
        private Edge edgePrefab;

        private GraphData graphData;
        public GraphData GetGraphData() => graphData;

        public override void Awake()
        {
            base.Awake();
            graphData = GetDataForThisLevel();
        }

        public override void OnStartServer()
        {
            Debug.Log("Server Start");
            base.OnStartServer();
            
            edgePrefab = Resources.Load<Edge>("Prefabs/Line");
            nodePrefab = Resources.Load<Node>("Prefabs/Node");
            

            uint nodeId = 0;
            foreach (var nodePos in graphData.NodesPositions)
            {
                var instance = Instantiate(nodePrefab);
                instance.Id = nodeId; 
                NetworkServer.Spawn(instance.gameObject);
                nodeId++;
            }

            foreach (var edge in graphData.Edges)
            {
                var instance = Instantiate(edgePrefab);
                NetworkServer.Spawn(instance.gameObject);
            }
        }
        
        
        public override void OnStartClient()
        {
            Debug.Log("ClientConnected");
            base.OnStartClient();
        }
        
        public override void OnClientDisconnect()
        {
            Debug.Log("ClientDisconnect");
            base.OnClientDisconnect();
        }
        
        private GraphData GetDataForThisLevel() => Resources.Load<GraphData>($"Levels/Level{levelNumber}");
    }
}