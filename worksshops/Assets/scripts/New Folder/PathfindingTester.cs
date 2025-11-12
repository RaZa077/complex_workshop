using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathfindingTester : MonoBehaviour
{
// The A* manager.
private AStarManager AStarManager = new AStarManager();
// List of possible waypoints.
private List<GameObject> Waypoints = new List<GameObject>();
// List of waypoint map connections. Represents a path.
private List<Connection> ConnectionArray = new List<Connection>();
// The start and end nodes.
[SerializeField]
private GameObject start;
[SerializeField]
private GameObject end;
    // Debug line offset.
    Vector3 OffSet = new Vector3(0, 0.3f, 0);
// Movement variables.
private float currentSpeed = 8;
private int currentTarget = 0;
private Vector3 currentTargetPos;
private int moveDirection = 1;
private bool agentMove = true;
// Start is called before the first frame update
void Start()
{
if (start == null || end == null)
{
Debug.Log("No start or end waypoints.");
return;
}
VisGraphWaypointManager tmpWpM = start.GetComponent<VisGraphWaypointManager>();
if (tmpWpM == null)
{
Debug.Log("Start is not a waypoint.");
return;
}
tmpWpM = end.GetComponent<VisGraphWaypointManager>();
if (tmpWpM == null)
{
Debug.Log("End is not a waypoint.");
return;
}// Find all the waypoints in the level.
GameObject[] GameObjectsWithWaypointTag;
GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("WayPoint");
foreach (GameObject waypoint in GameObjectsWithWaypointTag)
{
VisGraphWaypointManager tmpWaypointMan = waypoint.GetComponent<VisGraphWaypointManager>();
if (tmpWaypointMan)
{
Waypoints.Add(waypoint);
}
}
// Go through the waypoints and create connections.
foreach (GameObject waypoint in Waypoints)
{
VisGraphWaypointManager tmpWaypointMan = waypoint.GetComponent<VisGraphWaypointManager>();
// Loop through a waypoints connections.
foreach (VisGraphConnection aVisGraphConnection in tmpWaypointMan.Connections)
{
if (aVisGraphConnection.ToNode != null)
{
Connection aConnection = new Connection();
aConnection.FromNode = waypoint;
aConnection.ToNode = aVisGraphConnection.ToNode;
AStarManager.AddConnection(aConnection);
}
else
{
Debug.Log("Warning, " + waypoint.name + " has a missing to node for a connection!");
}
}
}
// Run A Star...
// ConnectionArray stores all the connections in the route to the goal / end node.
ConnectionArray = AStarManager.PathfindAStar(start, end);
if(ConnectionArray.Count == 0)
{
Debug.Log("Warning, A* did not return a path between the start and end node.");
}
}
// Draws debug objects in the editor and during editor play (if option set).
void OnDrawGizmos()
{
    // Draw path.
    if (ConnectionArray == null || ConnectionArray.Count == 0) return;

    foreach (Connection aConnection in ConnectionArray)
    {
        if (aConnection == null || aConnection.FromNode == null || aConnection.ToNode == null) continue;
        Gizmos.color = Color.white;
        Gizmos.DrawLine((aConnection.FromNode.transform.position + OffSet),
            (aConnection.ToNode.transform.position + OffSet));
    }
}
// Update is called once per frame
// Update is called once per frame
void Update()
{
    if (!agentMove) return;

    // Guard: make sure we have a valid path
    if (ConnectionArray == null || ConnectionArray.Count == 0) return;

    // Ensure currentTarget is within bounds before accessing
    currentTarget = Mathf.Clamp(currentTarget, 0, ConnectionArray.Count - 1);

    // Determine the direction to the current node in the array.
    if (moveDirection > 0)
    {
        if (ConnectionArray[currentTarget] == null || ConnectionArray[currentTarget].ToNode == null) return;
        currentTargetPos = ConnectionArray[currentTarget].ToNode.transform.position;
    }
    else
    {
        if (ConnectionArray[currentTarget] == null || ConnectionArray[currentTarget].FromNode == null) return;
        currentTargetPos = ConnectionArray[currentTarget].FromNode.transform.position;
    }

    // Clear y to avoid up/down movement. Assumes flat surface.
    currentTargetPos.y = transform.position.y;
    Vector3 direction = currentTargetPos - transform.position;

    // Calculate the length of the relative position vector
    float distance = direction.magnitude;

    // Face in the right direction.
    direction.y = 0;
    if (direction.sqrMagnitude > 0.000001f)
    {
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    // Safely compute normalized direction
    Vector3 normDirection = (distance > 0.000001f) ? direction / distance : Vector3.zero;

    // Move the game object.
    transform.position = transform.position + normDirection * currentSpeed * Time.deltaTime;

    // Check if close to current target.
    if (distance < 1f)
    {
        currentTarget += moveDirection;

        // If we've stepped beyond the end, reverse direction and step back into range
        if (currentTarget >= ConnectionArray.Count)
        {
            moveDirection = -1;
            currentTarget = ConnectionArray.Count - 1;
        }

        // If we've gone below zero, reverse and step into range
        if (currentTarget < 0)
        {
            moveDirection = 1;
            currentTarget = 0;
        }
    }
}
}