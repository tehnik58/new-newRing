using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{
    public static List<Waypoint> FindPath(List<Waypoint> allWaypoints, Waypoint start, Waypoint end)
    {
        // Защита от null/уничтоженных
        if (start == null || start.Equals(null) || end == null || end.Equals(null))
            return null;

        // Убираем уничтоженные объекты из списка
        allWaypoints = allWaypoints.Where(wp => wp != null && !wp.Equals(null)).ToList();

        Dictionary<Waypoint, float> distances = new();
        Dictionary<Waypoint, Waypoint> previous = new();
        HashSet<Waypoint> unvisited = new();

        foreach (var wp in allWaypoints)
        {
            distances[wp] = float.MaxValue;
            unvisited.Add(wp);
        }

        if (!distances.ContainsKey(start) || !distances.ContainsKey(end))
            return null;

        distances[start] = 0;

        while (unvisited.Count > 0)
        {
            Waypoint current = unvisited.OrderBy(wp => distances[wp]).FirstOrDefault();

            if (current == null || current.Equals(null))
            {
                unvisited.Remove(current);
                continue;
            }

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
                if (neighbor == null || neighbor.Equals(null)) continue;
                if (!unvisited.Contains(neighbor)) continue;

                float alt = distances[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

                if (!distances.ContainsKey(neighbor)) continue;

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