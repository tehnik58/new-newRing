using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingManager))]
public class PathfindingManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathfindingManager manager = (PathfindingManager)target;

        if (GUILayout.Button("Cache Current Path"))
        {
            manager.CacheCurrentPath();
        }
    }
}