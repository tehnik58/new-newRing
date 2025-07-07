using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    private List<Waypoint> _allWaypoints;

    public Route Route;

    void OnDrawGizmos()
    {
        _allWaypoints = WaypointsStorage.Waypoints.Values.ToList();
        DrawAllEgdes();
        DrawSelectedRoute();
    }

    private void DrawAllEgdes()
    {
        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        var drawnEdges = new HashSet<(Vector3, Vector3)>();

        foreach (Waypoint wp in _allWaypoints)
        {
            if (wp == null || wp.Equals(null)) continue;

            foreach (Waypoint connected in wp.connectedWaypoints)
            {
                if (connected == null || connected.Equals(null)) continue;

                var edge = (wp.transform.position, connected.transform.position);
                if (!drawnEdges.Contains(edge) && !drawnEdges.Contains((edge.Item2, edge.Item1)))
                {
                    Gizmos.DrawLine(edge.Item1, edge.Item2);
                    drawnEdges.Add(edge);
                }
            }
        }
    }

    private void DrawSelectedRoute()
    {
        if (!Route) return;
        
        Waypoint previous = null;
        foreach (var waypointName in Route.waypointsName)
        {
            if (WaypointsStorage.Waypoints.TryGetValue(waypointName, out Waypoint current))
            {
                if (previous != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(previous.transform.position, current.transform.position);
                }
                previous = current;
            }
        }
    }
}