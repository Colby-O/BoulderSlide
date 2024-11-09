using PlazmaGames.Core.Utils;
using PlazmaGames.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
	North,
	South,
	West,
	East
}

public class MapGenerator : MonoBehaviour
{
	[SerializeField] private Tilemap _tilemap;
	[SerializeField] private GenerationProperties _properties;

	private TileType[,] _grid;

	private void PlaceTiles()
	{
		for (int i = 0; i < _grid.GetLength(0); i++)
		{
			for (int j = 0; j < _grid.GetLength(1); j++)
			{
				TileBase tile = _properties.floor;

				if (_grid[i, j] == TileType.Floor) tile = _properties.floor;
				else if (_grid[i, j] == TileType.Hole) tile = _properties.hole;
				else if (_grid[i, j] == TileType.Wall) tile = _properties.wall;

				_tilemap.SetTile(new Vector3Int(i, j, 0), tile);
			}
		}
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

	private Vector2Int GetLastTile(Vector2Int cur, Direction dir)
	{
		if (dir == Direction.North)
		{
			return cur + new Vector2Int(0, -1);
		}
		else if (dir == Direction.South)
		{
			return cur + new Vector2Int(0, 1);
		}
		else if (dir == Direction.East)
		{
			return cur + new Vector2Int(-1, 0);
		}
		else
		{
			return cur + new Vector2Int(1, 0);
		}
	}

	private Direction GetDirection(Vector2Int cur, Direction? curDir = null)
	{
		int dir = -1;

		while (dir < 0)
		{
			dir = Random.Range(0, 4);

			Vector2Int next = GetNextTile(cur, (Direction)dir);
			Vector2Int next2 = GetNextTile(next, (Direction)dir);

			if 
			(
				next2.x < 0 ||
				next2.y < 0 ||
				next2.x >= _properties.size.x ||
				next2.y >= _properties.size.y ||
				_grid[next.x, next.y] == TileType.Wall || 
				_grid[next.x, next.y] == TileType.Start || 
				_grid[next2.x, next2.y] == TileType.Wall || 
				_grid[next2.x, next2.y] == TileType.Start ||
				_grid[next2.x, next2.y] == TileType.Hole ||
				curDir == (Direction)dir
			)
			{
				dir = -1;
			}
		}

		return (Direction)dir;
	}

	private bool CanMove(Vector2Int cur, Direction dir)
	{
		Vector2Int next = GetNextTile(cur, dir);
		Vector2Int next2 = GetNextTile(next, dir);

		if 
		(
			next2.x < 0 || 
			next2.y < 0 || 
			next2.x >= _properties.size.x || 
			next2.y >= _properties.size.y || 
			_grid[next.x, next.y] == TileType.Hole || 
			_grid[next.x, next.y] == TileType.Wall || 
			_grid[next2.x, next2.y] == TileType.Hole || 
			_grid[next2.x, next2.y] == TileType.Wall ||
			_grid[next2.x, next2.y] == TileType.Start ||
			_grid[next2.x, next2.y] == TileType.End
		) return false;

		return true;
	}

	private void GenerateLayout()
	{
		_grid = ArrayUtilities.CreateAndFill(_properties.size.x, _properties.size.y, TileType.Floor);

		for (int i = 0; i < _grid.GetLength(0); i++)
		{
			for (int j = 0; j < _grid.GetLength(1); j++)
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

		_grid[start.x, start.y] = TileType.Start;
		_grid[end.x, end.y] = TileType.End;

		Vector2Int cur = start;
		Direction curDir = GetDirection(start, null);
		int holesPlaced = 0;
		int numberOfMoves = 0;
		while (holesPlaced < _properties.minNumBoulders && cur != end)
		{
			if ((Random.value < 1 - _properties.turnProb || numberOfMoves <= 2) && CanMove(cur, curDir))
			{
				numberOfMoves++;
				cur = GetNextTile(cur, curDir);
			}
			else
			{
				holesPlaced++;
				if (_grid[GetNextTile(cur, curDir).x, GetNextTile(cur, curDir).y] == TileType.End)
				{
					_grid[cur.x, cur.y] = TileType.Hole;
					cur = GetLastTile(cur, curDir);
				}
				else
				{
					_grid[GetNextTile(cur, curDir).x, GetNextTile(cur, curDir).y] = TileType.Hole;
				}
				numberOfMoves = 0;
				curDir = GetDirection(cur, curDir);
			}
		}

		if (cur != end)
		{
			if (cur.y != end.y && end.y != _properties.size.y - 1)
			{
				int moves = -(cur.y - end.y);

				cur += new Vector2Int(0, moves);
				curDir = (moves > 0) ? Direction.North : Direction.South;
				_grid[GetNextTile(cur, curDir).x, GetNextTile(cur, curDir).y] = TileType.Hole;
			}
			else if (cur.x != end.x && end.x != _properties.size.x - 1)
			{
				int moves = -(cur.x - end.x);

				cur += new Vector2Int(moves, 0);
				curDir = (moves > 0) ? Direction.East : Direction.West;
				_grid[GetNextTile(cur, curDir).x, GetNextTile(cur, curDir).y] = TileType.Hole;
			}
		}
	}


	private void PlaceBoulders(Vector2Int loc)
	{
		Direction dir = GetDirection(loc);


	}

	private void GenerateBoulders()
	{
		for (int i = 0; i < _grid.GetLength(0); i++)
		{
			for (int j = 0; j < _grid.GetLength(1); j++)
			{
				if (_grid[i, j] == TileType.Hole)
				{
					PlaceBoulders(new Vector2Int(i, j));
				}
			}
		}
	}

	private void Awake()
	{
		GenerateLayout();
		PlaceTiles();
	}
}
