// AgentPathfinder.cs
using System.Collections.Generic;
using UnityEngine;

public class AgentPathfinder : MonoBehaviour
{
    [Header("UCS Setup")]
    [Tooltip("The starting waypoint for the path search.")]
    public VisGraphWaypointManager startWaypoint;
    [Tooltip("The goal waypoint for the path search.")]
    public VisGraphWaypointManager goalWaypoint;
    public float moveSpeed = 5f;

    // Data to store the path found by UCS for visualization
    public List<VisGraphWaypointManager> PathForVisualization { get; private set; } 

    private List<VisGraphWaypointManager> currentPath;
    private int currentWaypointIndex = 0;

    void Start()
    {
        // CONFIRMATION LOG: Should appear immediately in the console.
        Debug.Log("AgentPathfinder: Script started. Attempting UCS search now."); 

        // Execute the Uniform Cost Search
        currentPath = UniformCostSearch.FindPath(startWaypoint, goalWaypoint);
        currentWaypointIndex = 0;

        // Store the path for Gizmos drawing
        PathForVisualization = currentPath;
        
        if (currentPath == null)
        {
            Debug.LogError("AgentPathfinder: Failed to find a path. Check waypoint connections!");
        }
    }

    void Update()
    {
        // Only handles movement.
        if (currentPath != null && currentPath.Count > 0)
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        if (currentWaypointIndex >= currentPath.Count)
        {
            Debug.Log("Agent: Path execution complete!");
            currentPath = null;
            return;
        }

        VisGraphWaypointManager targetWaypoint = currentPath[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.transform.position;
        
        // 1. Move the agent towards the target waypoint
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            moveSpeed * Time.deltaTime
        );

        // 2. NEW LOGIC: Check if agent has reached the current target waypoint (2D distance check)
        
        // Create 2D vectors by ignoring the Y component (height)
        Vector3 agentPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPosXZ = new Vector3(targetPosition.x, 0, targetPosition.z);
        
        const float ReachTolerance = 0.5f; // Increased tolerance for reliability

        if (Vector3.Distance(agentPosXZ, targetPosXZ) < ReachTolerance)
        {
            // Advance to the next waypoint in the calculated path
            currentWaypointIndex++;
        }
    }

    // --- Path Visualization using Gizmos ---
    void OnDrawGizmos()
    {
        if (PathForVisualization != null && PathForVisualization.Count > 1)
        {
            DrawCalculatedPath(PathForVisualization);
        }
    }

    private void DrawCalculatedPath(List<VisGraphWaypointManager> path)
    {
        Gizmos.color = Color.green; // The color of the calculated path

        // Draw path lines and spheres for all but the last segment
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 startPos = path[i].transform.position;
            Vector3 endPos = path[i + 1].transform.position;

            Gizmos.DrawLine(startPos, endPos);
            Gizmos.DrawSphere(startPos, 0.2f);
        }
        
        // Draw sphere for the final waypoint
        if (path.Count > 0)
        {
            Gizmos.DrawSphere(path[path.Count - 1].transform.position, 0.2f);
        }

        // Highlight the agent's current target waypoint
        if (currentPath != null && currentWaypointIndex < currentPath.Count)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(currentPath[currentWaypointIndex].transform.position, 0.6f);
        }
    }
}