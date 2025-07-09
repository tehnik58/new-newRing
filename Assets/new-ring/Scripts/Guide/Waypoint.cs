using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
    public List<Waypoint> connectedWaypoints = new(); 
    public AudioClip guideVoice;

    void Awake()
    {
        UpdateBidirectionalConnections();
        WaypointsStorage.TryAddWaypoint(this);
    }
    
    void OnEnable()
    {
        UpdateBidirectionalConnections();
        WaypointsStorage.TryAddWaypoint(this);
    }

    void OnValidate()
    {
        UpdateBidirectionalConnections();
        WaypointsStorage.TryAddWaypoint(this);
    }

    void UpdateBidirectionalConnections()
    {
        // Добавляем обратные связи
        foreach (var wp in connectedWaypoints)
        {
            if (wp != null && !wp.Equals(null) && !wp.connectedWaypoints.Contains(this))
                wp.connectedWaypoints.Add(this);
        }

        // Удаляем недействительные или односторонние связи
        var toRemove = new List<Waypoint>();
        foreach (var wp in connectedWaypoints)
        {
            if (wp == null || wp.Equals(null) || !wp.connectedWaypoints.Contains(this))
                toRemove.Add(wp);
        }
        
        foreach (Waypoint wp in toRemove)
        {
            connectedWaypoints.Remove(wp);
        }
    }
}