using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RouteDatabase", menuName = "Pathfinding/Route Database")]
public class RouteDatabase : ScriptableObject
{
    [System.Serializable]
    public class NamedRoute
    {
        public string routeId;
        public List<Waypoint> waypoints = new();
    }

    public List<NamedRoute> routes = new();

    public List<Waypoint> GetRouteById(string id)
    {
        return routes.Find(r => r.routeId == id)?.waypoints;
    }
}
