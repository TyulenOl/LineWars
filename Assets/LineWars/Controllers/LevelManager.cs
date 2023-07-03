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

        private static readonly DirectoryInfo levelsDirectory;
        private static readonly DirectoryInfo assetsDirectory;
        
        private static readonly Node nodePrefab;
        private static readonly Edge edgePrefab;

        private static readonly List<Node> instantiatedNodes;

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

        // public static void LoadLevel(int mapIndex)
        // {
        //     Debug.Log("LoadingLevel");
        //     var graphData = Resources.Load<GraphData>($"Levels/Level{mapIndex}");
        //     var graphTransform = Graph.Instance.transform;
        //     
        //     foreach (var node in graphData.NodesPositions)
        //     {
        //         var instance = Object.Instantiate(nodePrefab,graphTransform);
        //         instance.Initialize();
        //         instance.transform.position = node;
        //         NetworkServer.Spawn(instance.gameObject);
        //         instantiatedNodes.Add(instance);
        //     }
        //
        //     foreach (var edge in graphData.Edges)
        //     {
        //         var instance = Object.Instantiate(edgePrefab,graphTransform);
        //         var node1 = instantiatedNodes[edge.Item1];
        //         var node2 = instantiatedNodes[edge.Item2];
        //         
        //         instance.Initialize(node1, node2);
        //         node1.AddEdge(instance);
        //         node2.AddEdge(instance);
        //         
        //         NetworkServer.Spawn(instance.gameObject);
        //     }
        //     Debug.Log("LevelLoaded");
        // }
        
        private static DirectoryInfo FindLevelsDirectory()
        {
            return FindAssetsDirectory()
                .EnumerateDirectories($"{LEVELS_DIRECTORY_NAME}", SearchOption.AllDirectories)
                .First();
        }
        
        private static DirectoryInfo FindAssetsDirectory() => new(Application.dataPath);
    }   
}