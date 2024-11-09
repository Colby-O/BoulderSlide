using PlazmaGames.Core.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapGeneratorOld : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private GenerationProperties _properties;

    private TileType[,] _grid;

    private Direction FetchDirection()
    {
        return (Direction)Random.Range(0, 4);
    }

    private Vector2Int GetNextTile(Vector2Int cur, Direction dir)
    {
        if (dir == Direction.North)
        {
            return cur + new Vector2Int(0, 1);
        }
        else if (dir == Direction.South)
        {
            return cur + new Vector2Int(0, -1);
        }
        else if (dir == Direction.East)
        {
            return cur + new Vector2Int(1, 0);
        }
        else
        {
            return cur + new Vector2Int(-1, 0);
        }
    }

    private bool CanMove(Vector2Int cur, Direction dir)
    {
        if (dir == Direction.North)
        {
            return GetNextTile(cur, dir).y < _properties.size.y - 3;
        }
        else if (dir == Direction.South)
        {
            return GetNextTile(cur, dir).y >= 2;
        }
        else if (dir == Direction.East)
        {
            return GetNextTile(cur, dir).x < _properties.size.x - 3;
        }
        else
        {
            return GetNextTile(cur, dir).x >= 2;
        }
    }

    private bool CanPlaceBoulder(Vector2Int cur, Direction dir)
    {
        if (dir == Direction.North)
        {
            return GetNextTile(cur, dir).y < _properties.size.y - 2;
        }
        else if (dir == Direction.South)
        {
            return GetNextTile(cur, dir).y >= 1;
        }
        else if (dir == Direction.East)
        {
            return GetNextTile(cur, dir).x < _properties.size.x - 2;
        }
        else
        {
            return GetNextTile(cur, dir).x >= 1;
        }
    }

    private bool IsWall(Vector2Int cur, Vector2Int end)
    {
        return cur.x == 0 || cur.y == 0 || (cur.x == _properties.size.x -1 && cur.x != end.x) || (cur.y == _properties.size.y - 1 && cur.y != end.y);
    }

    private void PlaceTiles()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                TileBase tile = _properties.floor;

                if (_grid[i, j] == TileType.None) tile = _properties.floor;
                else if (_grid[i, j] == TileType.Hole) tile = _properties.hole;
                else if (_grid[i, j] == TileType.Wall) tile = _properties.wall;

                _tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }

    private void GenerateLayout()
    {
        _grid = ArrayUtilities.CreateAndFill(_properties.size.x, _properties.size.y, TileType.None);

        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0;  j < _grid.GetLength(1); j++)
            {
                if (i == 0 || j == 0 || i == _grid.GetLength(0) - 1 || j == _grid.GetLength(1) - 1)
                {
                    _grid[i, j] = TileType.Wall;
                }
            }
        }

        Vector2Int start = new Vector2Int(1, 0);
        float rand = Random.value;
        Vector2Int end = new Vector2Int(
            (rand >= 0.5f) ? _grid.GetLength(0) - 3 : _grid.GetLength(0) - 1, 
            (rand >= 0.5f) ? _grid.GetLength(1) - 1 : _grid.GetLength(1) - 3
        );


        _grid[start.x, start.y] = TileType.None;
        _grid[end.x, end.y] = TileType.None;

        int bolders = 0;
        Vector2Int cur = start;
        Direction currentDirection = FetchDirection();

        int numberOfMoves = 0;
        while (bolders <= _properties.minNumBoulders && cur != end)
        {
            if 
            (
                (Random.value < 1 - _properties.turnProb || numberOfMoves <= 2) && 
                CanMove(cur, currentDirection) && 
                _grid[GetNextTile(cur, currentDirection).x, GetNextTile(cur, currentDirection).y] != TileType.Hole
            )
            {
                numberOfMoves++;
                cur = GetNextTile(cur, currentDirection);
            }
            else
            {
                if (CanPlaceBoulder(cur, currentDirection) && cur != start)
                {
                    bolders++;
                    _grid[GetNextTile(cur, currentDirection).x, GetNextTile(cur, currentDirection).y] = TileType.Hole;
                }
                numberOfMoves = 0;
                Direction next = FetchDirection();
                while (next == currentDirection || IsWall(GetNextTile(cur, next), end)) next = FetchDirection();
                currentDirection = next;
            }
        }

        if (cur != end)
        {
            if (cur.x != end.x)
            {
                int moves = -(cur.x - end.x);

                cur += new Vector2Int(moves, 0);
                currentDirection = (moves > 0) ? Direction.North : Direction.South;
                _grid[GetNextTile(cur, currentDirection).x, GetNextTile(cur, currentDirection).y] = TileType.Hole;
            }

            if (cur.y != end.y)
            {
                int moves = -(cur.y - end.y);

                cur += new Vector2Int(0, moves);
                currentDirection = (moves > 0) ? Direction.West : Direction.East;
                _grid[GetNextTile(cur, currentDirection).x, GetNextTile(cur, currentDirection).y] = TileType.Hole;
            }
        }

        PlaceTiles();
    }

    private void Awake()
    {
        GenerateLayout();
    }


}
