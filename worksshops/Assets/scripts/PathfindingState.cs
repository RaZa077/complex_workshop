// PathfindingState.cs
public class PathfindingState
{
    public VisGraphWaypointManager Node;
    public float CostSoFar; // g(n): The accumulated cost from the start node

    public PathfindingState(VisGraphWaypointManager node, float cost)
    {
        Node = node;
        CostSoFar = cost;
    }
}