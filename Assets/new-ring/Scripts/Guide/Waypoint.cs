using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour
{
    public List<Waypoint> connectedWaypoints = new(); 
    public AudioClip guideVoice;

    void OnEnable()
    {
        UpdateBidirectionalConnections();
    }

    void OnValidate()
    {
        UpdateBidirectionalConnections();
    }

    void UpdateBidirectionalConnections()
    {
        // Добавляем обратные связи
        foreach (var wp in connectedWaypoints)
            if (wp != null && !wp.connectedWaypoints.Contains(this))
                wp.connectedWaypoints.Add(this);
        
        // Удаляем обратные связи, если они были удалены
        var toRemove = new List<Waypoint>();
        foreach (var wp in connectedWaypoints)
            if (wp == null || !wp.connectedWaypoints.Contains(this))
                toRemove.Add(wp);
        foreach (Waypoint wp in toRemove)
            connectedWaypoints.Remove(wp);
    }
}