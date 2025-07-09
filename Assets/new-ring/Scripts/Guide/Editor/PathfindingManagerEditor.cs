using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingManager))]
public class PathfindingManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var manager = (PathfindingManager)target;

        if (GUILayout.Button("Cache Current Path"))
        {
            if(manager.PathName != null) 
                PathCacher.CacheCurrentPath(manager.finalPath, manager.PathName);
        }
    }
}