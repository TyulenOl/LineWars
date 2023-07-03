using System.IO;
using System.Linq;
using Controllers;
using UnityEditor;
using UnityEngine;

namespace Model.Graph
{
    [CustomEditor(typeof(Graph))]
    public class GraphEditor: Editor
    {
        private DirectoryInfo assetsDirectory;
        private DirectoryInfo levelsDirectory;
        private Graph Graph => (Graph) target;
        private void OnEnable()
        {
            FindLevelsDirectory();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Save"))
            {
                SaveGraph();
            }
        }

        private void SaveGraph()
        {
            var graphData = CreateInstance<GraphData>();
            var allNodes = Graph.GetNodes().ToArray();
            graphData.Initialize(allNodes);

            var uniqueAssetFileName = GetUniqueRelativePath();
            
            AssetDatabase.CreateAsset(graphData, uniqueAssetFileName);
        }

        private string GetUniqueRelativePath()
        {
            var relativePath = Path.GetRelativePath(assetsDirectory.Parent.FullName, levelsDirectory.FullName);
            var assetFileName = Path.Join(relativePath, LevelManager.LEVEL_FILE_NAME);
            var uniqueAssetFileName = AssetDatabase
                .GenerateUniqueAssetPath(assetFileName);
            return uniqueAssetFileName;
        }

        private void FindLevelsDirectory()
        {
            assetsDirectory = LevelManager.AssetsDirectory;
            levelsDirectory = LevelManager.LevelsDirectory;
        }
    }
}