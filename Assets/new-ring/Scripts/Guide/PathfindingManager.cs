using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class PathfindingManager : MonoBehaviour
{
    public List<Waypoint> keyWaypoints = new();
    private List<Waypoint> allWaypoints;
    public List<List<Waypoint>> FinalPath { get; private set; } = new();

    void OnEnable() => Refresh();
    void OnValidate() => Refresh();
    void Update() => Refresh();

    private void Refresh()
    {
        UpdateWaypoints();
        CalculatePath();
    }

    private void UpdateWaypoints()
    {
        allWaypoints = FindObjectsOfType<Waypoint>().ToList();
        keyWaypoints.RemoveAll(wp => wp == null);
    }

    private void CalculatePath()
    {
        FinalPath.Clear();
        if (keyWaypoints.Count < 2) return;

        for (int i = 0; i < keyWaypoints.Count - 1; i++)
        {
            var path = Pathfinder.FindPath(allWaypoints, keyWaypoints[i], keyWaypoints[i + 1]);
            if (path != null) FinalPath.Add(path);
        }
    }

    void OnDrawGizmos()
    {
        if (allWaypoints == null) return;

        Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        var drawnEdges = new HashSet<(Vector3, Vector3)>();

        foreach (Waypoint wp in allWaypoints)
        {
            foreach (Waypoint connected in wp.connectedWaypoints)
            {
                var edge = (wp.transform.position, connected.transform.position);
                if (!drawnEdges.Contains(edge) && !drawnEdges.Contains((edge.Item2, edge.Item1)))
                {
                    Gizmos.DrawLine(edge.Item1, edge.Item2);
                    drawnEdges.Add(edge);
                }
            }
        }

        Gizmos.color = Color.red;
        foreach (var segment in FinalPath)
        {
            for (int i = 0; i < segment.Count - 1; i++)
            {
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
