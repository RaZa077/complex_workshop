// // VisGraphConnection.cs
// using UnityEngine;
// using System;

// [Serializable]
// public class VisGraphConnection
// {
//     // Must reference the waypoint script, not just the generic GameObject
//     [SerializeField]
//     private VisGraphWaypointManager toNode;
    
//     // The cost for UCS to use. If 0, it defaults to distance.
//     [SerializeField]
//     private float cost = 0f;

//     public VisGraphWaypointManager ToNode
//     {
//         get { return toNode; }
//     }
    
//     public float Cost
//     {
//         get { return cost; }
//         set { cost = value; }
//     }
// }