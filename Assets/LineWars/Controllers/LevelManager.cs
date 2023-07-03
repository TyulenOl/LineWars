using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mirror;
using Model.Graph;
using UnityEditor;
using UnityEngine;

namespace Controllers
{
    public static class LevelManager
    {
        public const string LEVELS_DIRECTORY_NAME = "Levels";
        public const string LEVEL_FILE_NAME = "Level.asset";

        private static DirectoryInfo levelsDirectory;
        private static DirectoryInfo assetsDirectory;
        
        private static Node nodePrefab;
        private static Edge edgePrefab;

        private static List<Node> instantiatedNodes;

        public static DirectoryInfo LevelsDirectory => levelsDirectory;
        public static DirectoryInfo AssetsDirectory => assetsDirectory;
        
        static LevelManager()
        {
            levelsDirectory = FindLevelsDirectory();
            assetsDirectory = FindAssetsDirectory();

            edgePrefab = Resources.Load<Edge>("Prefabs/Line");
            nodePrefab = Resources.Load<Node>("Prefabs/Node");

            instantiatedNodes = new List<Node>();
        }

        public static void LoadLevel(int mapIndex)
        {
            var graphData = Resources.Load<GraphData>($"Levels/Level{mapIndex}");
            var graphTransform = Graph.Instance.transform;
            foreach (var node in graphData.NodesPositions)
            {
                var instance = Object.Instantiate(nodePrefab,graphTransform);
                instance.Initialize();
                instance.transform.position = node;
                NetworkServer.Spawn(instance.gameObject);
                instantiatedNodes.Add(instance);
            }

            foreach (var edge in graphData.Edges)
            {
                var instance = Object.Instantiate(edgePrefab,graphTransform);
                var node1 = instantiatedNodes[edge.Item1];
                var node2 = instantiatedNodes[edge.Item2];
                
                instance.Initialize(node1, node2);
                node1.AddEdge(instance);
                node2.AddEdge(instance);
                
                NetworkServer.Spawn(instance.gameObject);
            }
        }
        
        private static DirectoryInfo FindLevelsDirectory()
        {
            return FindAssetsDirectory()
                .EnumerateDirectories($"{LEVELS_DIRECTORY_NAME}", SearchOption.AllDirectories)
                .First();
        }
        
        private static DirectoryInfo FindAssetsDirectory()
        {
            return new DirectoryInfo(Application.dataPath);
        }
    }   
}