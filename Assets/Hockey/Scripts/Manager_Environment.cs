using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class Manager_Environment
{
    private static Manager_Environment instance_EM;
    private List<GameObject> waypoints = new List<GameObject>();
    public List<GameObject> Waypoints { get { return waypoints; } }

    public static Manager_Environment Singleton
    {
        get
        {
            if(instance_EM == null)
            {
                instance_EM = new Manager_Environment();
                instance_EM.Waypoints.AddRange(
                    GameObject.FindGameObjectsWithTag("Waypoint"));
                //Order Waypoints by Alphabetical ascending
                instance_EM.waypoints = instance_EM.waypoints.OrderBy(waypoint => waypoint.name).ToList();
            }
            return instance_EM;
        }
    }

}
