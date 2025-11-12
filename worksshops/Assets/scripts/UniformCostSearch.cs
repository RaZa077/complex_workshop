// // UniformCostSearch.cs
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq; // Used for ordering (simulating a Priority Queue)

// public static class UniformCostSearch
// {
//     public static List<VisGraphWaypointManager> FindPath(
//         VisGraphWaypointManager startNode, 
//         VisGraphWaypointManager goalNode)
//     {
//         if (startNode == null || goalNode == null)
//         {
//             Debug.LogError("UCS: Start or Goal node is null. Check AgentPathfinder setup.");
//             return null;
//         }

//         // Data structures required for UCS
//         List<PathfindingState> frontier = new List<PathfindingState>(); // The Priority Queue
//         Dictionary<VisGraphWaypointManager, float> costSoFar = new Dictionary<VisGraphWaypointManager, float>();
//         Dictionary<VisGraphWaypointManager, VisGraphWaypointManager> cameFrom = 
//             new Dictionary<VisGraphWaypointManager, VisGraphWaypointManager>();

//         // Initialization
//         frontier.Add(new PathfindingState(startNode, 0f));
//         costSoFar[startNode] = 0f;

//         // Search Loop
//         while (frontier.Count > 0)
//         {
//             // UCS CORE: Get the node with the lowest accumulated cost
//             PathfindingState currentPathState = frontier.OrderBy(p => p.CostSoFar).First();
//             frontier.Remove(currentPathState);

//             VisGraphWaypointManager currentNode = currentPathState.Node;

//             // Goal Check
//             if (currentNode == goalNode)
//             {
//                 Debug.Log($"UCS: Path found with total cost: {currentPathState.CostSoFar}");
//                 return ReconstructPath(cameFrom, startNode, goalNode);
//             }

//             // Explore Neighbors
//             foreach (var connection in currentNode.Connections)
//             {
//                 VisGraphWaypointManager neighbor = connection.ToNode;

//                 if (neighbor == null) continue;

//                 // Determine the transition cost
//                 float transitionCost = connection.Cost; 
//                 if (transitionCost <= 0.001f) // Use distance if manual cost is zero/unset
//                 {
//                     transitionCost = Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
//                 }

//                 // Calculate the new accumulated cost
//                 float newCost = costSoFar[currentNode] + transitionCost;

//                 // UCS Check: Is this a cheaper path to the neighbor?
//                 if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
//                 {
//                     // Found a better path!
//                     costSoFar[neighbor] = newCost;
//                     cameFrom[neighbor] = currentNode;
                    
//                     // Add/Update the neighbor in the frontier
//                     frontier.Add(new PathfindingState(neighbor, newCost));
//                 }
//             }
//         }

//         Debug.LogWarning("UCS: Search failed to find a path.");
//         return null;
//     }

//     // Path Reconstruction Helper
//     private static List<VisGraphWaypointManager> ReconstructPath(
//         Dictionary<VisGraphWaypointManager, VisGraphWaypointManager> cameFrom, 
//         VisGraphWaypointManager start, VisGraphWaypointManager goal)
//     {
//         List<VisGraphWaypointManager> path = new List<VisGraphWaypointManager>();
//         VisGraphWaypointManager current = goal;

//         while (current != start)
//         {
//             if (!cameFrom.ContainsKey(current)) return null;
//             path.Add(current);
//             current = cameFrom[current];
//         }

//         path.Add(start);
//         path.Reverse();
//         return path;
//     }
// }