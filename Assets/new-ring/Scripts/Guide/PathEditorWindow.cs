#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathEditorWindow : EditorWindow
{
    private RouteDatabase database;
    private string newRouteId;

    [MenuItem("Tools/Route Editor")]
    public static void ShowWindow()
    {
        GetWindow<PathEditorWindow>("Route Editor");
    }

    void OnGUI()
    {
        database = (RouteDatabase)EditorGUILayout.ObjectField("Route DB", database, typeof(RouteDatabase), false);
        newRouteId = EditorGUILayout.TextField("New Route ID", newRouteId);

        if (GUILayout.Button("Add Route from PathfindingManager"))
        {
            var manager = FindObjectOfType<PathfindingManager>();
            if (manager != null && database != null)
            {
                var flatPath = new List<Waypoint>();
                foreach (var segment in manager.FinalPath)
                    flatPath.AddRange(segment);

                database.routes.Add(new RouteDatabase.NamedRoute
                {
                    routeId = newRouteId,
                    waypoints = flatPath
                });

                EditorUtility.SetDirty(database);
            }
        }
    }
}
#endif