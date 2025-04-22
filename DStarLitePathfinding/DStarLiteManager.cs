using System.Collections.Generic;
using UnityEngine;

public class DStarLiteManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    private Node[,] map;
    private Node start, goal;
    private float km = 0f;
    private PriorityQueue<Node> openList = new();

    void Start()
    {
        InitMap();
        ComputeShortestPath();
        PrintPath();
    }

    void InitMap()
    {
        map = new Node[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                map[x, y] = new Node(new Vector2Int(x, y));

        start = map[0, 0];
        goal = map[width - 1, height - 1];
        goal.RHS = 0;
        openList.Enqueue(goal, goal.Key(start, km));
    }

    public void AddObstacle(Vector2Int pos)
    {
        var n = map[pos.x, pos.y];
        n.IsObstacle = true;
        foreach (var s in GetNeighbors(n))
            UpdateVertex(s);
    }

    void UpdateVertex(Node u)
    {
        if (u != goal)
        {
            float min = float.PositiveInfinity;
            foreach (var s in GetNeighbors(u))
                min = Mathf.Min(min, s.G + 1);
            u.RHS = min;
        }

        openList.Remove(u);
        if (u.G != u.RHS)
            openList.Enqueue(u, u.Key(start, km));
    }

    void ComputeShortestPath()
    {
        while (openList.Count > 0 &&
               (LessThan(openList.Peek().Key(start, km), start.Key(start, km)) || start.RHS != start.G))
        {
            Node u = openList.Dequeue();
            if (u.G > u.RHS)
            {
                u.G = u.RHS;
                foreach (var s in GetNeighbors(u))
                    UpdateVertex(s);
            }
            else
            {
                u.G = float.PositiveInfinity;
                UpdateVertex(u);
                foreach (var s in GetNeighbors(u))
                    UpdateVertex(s);
            }
        }
    }

    bool LessThan((float, float) a, (float, float) b)
    {
        return a.Item1 < b.Item1 || (a.Item1 == b.Item1 && a.Item2 < b.Item2);
    }

    List<Node> GetNeighbors(Node n)
    {
        var list = new List<Node>();
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var dir in dirs)
        {
            Vector2Int np = n.Pos + dir;
            if (np.x >= 0 && np.y >= 0 && np.x < width && np.y < height)
            {
                Node neighbor = map[np.x, np.y];
                if (!neighbor.IsObstacle)
                    list.Add(neighbor);
            }
        }
        return list;
    }

    public void PrintPath()
    {
        var current = start;
        Debug.Log("Path:");
        while (current != goal)
        {
            Debug.Log(current.Pos);
            Node next = null;
            float min = float.PositiveInfinity;
            foreach (var s in GetNeighbors(current))
            {
                float cost = s.G + 1;
                if (cost < min)
                {
                    min = cost;
                    next = s;
                }
            }
            if (next == null) break;
            current = next;
        }
        Debug.Log(goal.Pos);
    }
}
