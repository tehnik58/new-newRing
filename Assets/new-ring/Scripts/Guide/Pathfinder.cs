using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    public static List<Waypoint> FindPath(List<Waypoint> allWaypoints, Waypoint start, Waypoint end)
    {
        Dictionary<Waypoint, float> distances = allWaypoints.ToDictionary(wp => wp, wp => float.MaxValue);
        Dictionary<Waypoint, Waypoint> previous = new();
        var unvisited = new HashSet<Waypoint>(allWaypoints);

        distances[start] = 0;

        while (unvisited.Count > 0)
        {
            Waypoint current = unvisited.OrderBy(wp => distances[wp]).First();
            unvisited.Remove(current);

            if (current == end)
            {
                List<Waypoint> path = new();
                while (current != null)
                {
                    path.Add(current);
                    previous.TryGetValue(current, out current);
                }
                path.Reverse();
                return path;
            }

            foreach (var neighbor in current.connectedWaypoints)
            {
                float alt = distances[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                }
            }
        }

        return null;
    }
}