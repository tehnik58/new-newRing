using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class PathfindingManager : MonoBehaviour
{
    [SerializeField] private List<Waypoint> keyWaypoints = new();
    [SerializeField] private string pathName;
    private List<Waypoint> _allWaypoints;

    public string PathName => pathName;
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
        keyWaypoints.RemoveAll(wp => !wp || wp.Equals(null));
        _allWaypoints = WaypointsStorage.Waypoints.Values.ToList();
        CalculatePath();
    }

    private void CalculatePath()
    {
        finalPath.Clear();
        if (keyWaypoints.Count < 2) return;

        for (int i = 0; i < keyWaypoints.Count - 1; i++)
        {
            var start = keyWaypoints[i];
            var end = keyWaypoints[i + 1];

            if (!start || start.Equals(null) || !end || end.Equals(null)) continue;

            var path = Pathfinder.FindPath(_allWaypoints, start, end);
            if (path != null)
                finalPath.Add(path);
        }
    }

    private void OnDrawGizmos()
    {
        var radius = 0.5f;
        
        // Highlight first key waypoint (red sphere) even if alone
        if (keyWaypoints.Count > 0 && keyWaypoints[0] != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(keyWaypoints[0].transform.position, radius);
        }

        Gizmos.color = Color.red;
        foreach (var segment in finalPath)
        {
            for (int i = 0; i < segment.Count - 1; i++)
            {
                if (segment[i] == null || segment[i + 1] == null) continue;

                Vector3 a = segment[i].transform.position;
                Vector3 b = segment[i + 1].transform.position;
                Gizmos.DrawLine(a, b);
            }

            // Highlight last waypoint in segment (blue sphere)
            if (segment.Count > 0 && segment[segment.Count - 1] != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(segment[segment.Count - 1].transform.position, radius);
            }
        }
    }
}