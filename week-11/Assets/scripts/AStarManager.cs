using System.Collections.Generic;
using UnityEngine;

public class AStarManager
{
    public List<Connection> connections = new List<Connection>();

    public void AddConnection(Connection c)
    {
        if (!connections.Contains(c))
            connections.Add(c);
    }

    public List<Connection> GetConnections(GameObject node)
    {
        List<Connection> conList = new List<Connection>();
        foreach (Connection c in connections)
            if (c.FromNode == node)
                conList.Add(c);
        return conList;
    }

    float Heuristic(GameObject a, GameObject b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    public List<Connection> PathfindAStar(GameObject start, GameObject goal)
    {
        List<GameObject> open = new List<GameObject>();
        List<GameObject> closed = new List<GameObject>();
        Dictionary<GameObject, float> gScore = new Dictionary<GameObject, float>();
        Dictionary<GameObject, float> fScore = new Dictionary<GameObject, float>();
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();

        open.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (open.Count > 0)
        {
            GameObject current = open[0];
            foreach (var n in open)
                if (fScore.ContainsKey(n) && fScore[n] < fScore[current])
                    current = n;

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            open.Remove(current);
            closed.Add(current);

            foreach (var con in GetConnections(current))
            {
                GameObject neighbor = con.ToNode;
                if (closed.Contains(neighbor)) continue;

                float tentative_g = gScore[current] + con.Cost;

                if (!open.Contains(neighbor))
                    open.Add(neighbor);
                else if (gScore.ContainsKey(neighbor) && tentative_g >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_g;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
            }
        }

        Debug.LogWarning("A* returned NO PATH!");
        return new List<Connection>();
    }

    List<Connection> ReconstructPath(Dictionary<GameObject, GameObject> cameFrom, GameObject current)
    {
        List<Connection> result = new List<Connection>();
        while (cameFrom.ContainsKey(current))
        {
            GameObject prev = cameFrom[current];
            Connection c = new Connection { FromNode = prev, ToNode = current };
            result.Insert(0, c);
            current = prev;
        }
        return result;
    }
}
