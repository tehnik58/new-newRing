using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class PathfindingManager : MonoBehaviour
{
    private List<Waypoint> _allWaypoints;
    public List<Waypoint> keyWaypoints = new();
    public List<List<Waypoint>> finalPath { get; } = new();

    void OnEnable() => Refresh();
    void OnValidate() => Refresh();

    void Update()
    {
        if (Application.isPlaying) return;
        Refresh();
    }

    private void Refresh()
    {
        CleanKeyWaypoints();
        UpdateWaypoints();
        CalculatePath();
    }

    private void UpdateWaypoints()
    {
        _allWaypoints = FindObjectsOfType<Waypoint>().Where(wp => wp != null).ToList();
    }

    private void CleanKeyWaypoints()
    {
        keyWaypoints.RemoveAll(wp => wp == null || wp.Equals(null));
    }

    private void CalculatePath()
    {
        finalPath.Clear();
        if (keyWaypoints.Count < 2) return;

        for (int i = 0; i < keyWaypoints.Count - 1; i++)
        {
            var start = keyWaypoints[i];
            var end = keyWaypoints[i + 1];

            if (start == null || start.Equals(null) || end == null || end.Equals(null)) continue;

            var path = Pathfinder.FindPath(_allWaypoints, start, end);
            if (path != null) finalPath.Add(path);
        }
    }

    void OnDrawGizmos()
    {
        if (_allWaypoints == null) return;

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

        Gizmos.color = Color.red;
        foreach (var segment in finalPath)
        {
            for (int i = 0; i < segment.Count - 1; i++)
            {
                if (segment[i] == null || segment[i + 1] == null) continue;

                Vector3 a = segment[i].transform.position;
                Vector3 b = segment[i + 1].transform.position;
                Vector3 offset = (b - a).normalized * 0.05f;
                Gizmos.DrawLine(a + offset, b + offset);
                Gizmos.DrawLine(a - offset, b - offset);
                Gizmos.DrawLine(a, b);
            }
        }
    }
}
