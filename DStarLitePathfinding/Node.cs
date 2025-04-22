using UnityEngine;

public class Node
{
    public Vector2Int Pos;
    public float G = float.PositiveInfinity;
    public float RHS = float.PositiveInfinity;
    public bool IsObstacle = false;

    public Node(Vector2Int pos)
    {
        this.Pos = pos;
    }

    public (float, float) Key(Node start, float km)
    {
        float h = Heuristic(Pos, start.Pos);
        float min = Mathf.Min(G, RHS);
        return (min + h + km, min);
    }

    public static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
