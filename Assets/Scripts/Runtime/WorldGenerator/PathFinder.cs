using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class PathFinder
{
    private TileType[,] _grid;
    private readonly int _gridSize;
    private Dictionary<Vector2Int, Vector2Int> _cameFrom;

    private static readonly Vector2Int[] DIRECTIONS = new[]
    {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

    /// <summary>
    /// Constructor given a grid and tile weights
    /// </summary>
    public PathFinder(TileType[,] _grid)
    {
        this._grid = _grid;
        this._gridSize = _grid.GetLength(0);
    }

    /// <summary>
    /// Finds and returns the neighbouring tiles
    /// </summary>
    private List<Vector2Int> GetNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new();

        foreach (Vector2Int dir in DIRECTIONS)
        {
            Vector2Int newGridTile = current + dir;
            if (newGridTile.x < _gridSize && newGridTile.y < _gridSize && newGridTile.x >= 0 && newGridTile.y >= 0) neighbors.Add(newGridTile);
        }

        return neighbors;
    }

    /// <summary>
    /// Gets the current cost of the path
    /// </summary>
    private float GetCurrentCost(Vector2Int previous, Vector2Int next)
    {
        float weight = Mathf.Infinity;

        switch (_grid[next.x, next.y])
        {
            case TileType.None:
                weight = 5;
                break;
            case TileType.Push:
                weight = 0;
                break;
            case TileType.Floor:
                weight = 0;
                break;
            default:
                weight = Mathf.Infinity;
                break;
        }

        return weight;
    }

    /// <summary>
    /// Computes the Manhattan Distacne
    /// </summary>
    private float GetManhattanDistance(Vector2Int goal, Vector2Int next)
    {
        return Mathf.Abs(goal.x - next.x) + Mathf.Abs(goal.y - next.y);
    }

    /// <summary>
    /// Finds the lowest cost path using A* with the Manhattan Distacne as the huristic.
    /// </summary>
    public Dictionary<Vector2Int, Vector2Int> FindOptimalPath(Vector2Int start, Vector2Int goal)
    {
        PriorityQueue<Vector2Int, float> fontier = new();
        Dictionary<Vector2Int, float> costSoFar = new();
        Dictionary<Vector2Int, Vector2Int> _cameFrom = new();

        fontier.Enqueue(start, 0.0f);
        costSoFar[start] = 0.0f;

        while (fontier.Count != 0)
        {
            Vector2Int current = fontier.Dequeue();
            Vector2Int previous = (_cameFrom.ContainsKey(current)) ? _cameFrom[current] : current;

            if (current == goal) break;

            foreach (Vector2Int next in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + GetCurrentCost(previous, next);
                if ((!costSoFar.ContainsKey(next) || newCost < costSoFar[next]))
                {
                    costSoFar[next] = newCost;
                    // Adds Heuristic Cost
                    float priority = newCost + GetManhattanDistance(goal, next);
                    fontier.Enqueue(next, priority);
                    _cameFrom[next] = current;
                }
            }
        }

        this._cameFrom = _cameFrom;

        return _cameFrom;
    }

    /// <summary>
    /// Draws a path for degubbing purposes
    /// </summary>
    public void DrawPath(Vector2Int start, float _gridSize = 1, Color? color = null, float duration = Mathf.Infinity)
    {
        Vector2Int pt = start;
        while (true)
        {
            if (!_cameFrom.ContainsKey(pt)) break;

            Vector3 worldPT = new Vector3(pt.x, 0, pt.y) * _gridSize;
            Vector3 nextWorldPT = new Vector3(_cameFrom[pt].x, 0, _cameFrom[pt].y) * _gridSize;

            Debug.DrawLine(worldPT, nextWorldPT, color ?? Color.magenta, duration);
            pt = _cameFrom[pt];
        }
    }
}

