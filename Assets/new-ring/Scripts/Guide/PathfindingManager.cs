using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class PathfindingManager : MonoBehaviour
{
    private List<Waypoint> _allWaypoints;
    public Dictionary<string, Waypoint> WaypointDictionary = new();
    
    public PathDrawer PathDrawer;
    public List<Waypoint> keyWaypoints = new();
    public List<List<Waypoint>> FinalPath = new();
    public string PathName;
    
    void OnEnable() => Refresh();

    void OnValidate()
    {
        Refresh();
        if(PathDrawer)
            PathDrawer.AllWaypoints = _allWaypoints;
    }
    
    void Update()
    {
        if (Application.isPlaying)
            return;
        Refresh();
    }

    private void Refresh()
    {
        if(!PathDrawer) return;
        keyWaypoints.RemoveAll(wp => !wp || wp.Equals(null));
        UpdateWaypoints();
        CalculatePath();
    }
    

    private void UpdateWaypoints()
    {
        _allWaypoints = FindObjectsOfType<Waypoint>().Where(wp => wp).ToList();
        WaypointDictionary = _allWaypoints.ToDictionary(wp => wp.name, wp => wp);
    }


    private void CalculatePath()
    {
        FinalPath.Clear();
        if (keyWaypoints.Count < 2) return;

        for (int i = 0; i < keyWaypoints.Count - 1; i++)
        {
            var start = keyWaypoints[i];
            var end = keyWaypoints[i + 1];

            if (!start || start.Equals(null) || !end || end.Equals(null)) continue;

            var path = Pathfinder.FindPath(_allWaypoints, start, end);
            if (path != null) 
                FinalPath.Add(path);
        }
    }

    public void CacheCurrentPath()
    {
        if(PathName != null) 
            PathCacher.CacheCurrentPath(FinalPath, PathName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var segment in FinalPath)
        {
            for (int i = 0; i < segment.Count - 1; i++)
            {
                if (segment[i] == null || segment[i + 1] == null) continue;
        
                Vector3 a = segment[i].transform.position;
                Vector3 b = segment[i + 1].transform.position;
                // Vector3 offset = (b - a).normalized * 0.05f;
                // Gizmos.DrawLine(a + offset, b + offset);
                // Gizmos.DrawLine(a - offset, b - offset);
                Gizmos.DrawLine(a, b);
            }
        }
    }
}