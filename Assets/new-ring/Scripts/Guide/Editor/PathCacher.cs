using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class PathCacher
{
    public static void CacheCurrentPath(List<List<Waypoint>> finalPath, string name)
    {
        var listWaypoints = new List<string>();
        foreach (var segment in finalPath)
        {
            foreach (var wp in segment)
            {
                if (wp)
                    listWaypoints.Add(wp.gameObject.name);
            }
        }

        // Create SO Route
        var route = ScriptableObject.CreateInstance<Route>();
        route.routeId = $"Route_{name}";
        route.waypointsName = listWaypoints;
        route.name = $"Route_{name}";

        // Check if route already exists
        var path = "Assets/new-ring/Scripts/Guide/Routes/" + route.routeId + ".asset";
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("Overwrite Route", $"Route '{route.routeId}' already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                return;
            }
            AssetDatabase.DeleteAsset(path);
        }

        // Ensure directory exists
        var dirPath = "Assets/new-ring/Scripts/Guide/Routes";
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        // Save SO as asset
        AssetDatabase.CreateAsset(route, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}