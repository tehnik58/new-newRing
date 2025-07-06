using System.Collections.Generic;
using UnityEngine;

public class Route : ScriptableObject
{
    public string routeId;
    public List<string> waypointsName = new();
}