using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointsStorage : MonoBehaviour
{
    private static Dictionary<string, Waypoint> _waypoints = new();
    public static IReadOnlyDictionary<string, Waypoint> Waypoints => _waypoints;

    private void Awake()
    {
        DeleteVoidWaypoints();
    }

    private static void DeleteVoidWaypoints()
    {
        var keysToRemove = _waypoints
            .Where(kvp => kvp.Value == null)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _waypoints.Remove(key);
        }
    }

    public static void TryAddWaypoint(Waypoint waypoint)
    {
        if(waypoint == null) return;
        _waypoints.TryAdd(waypoint.name, waypoint);
        DeleteVoidWaypoints();
    }
}