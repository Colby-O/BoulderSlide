using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core;
using PlazmaGames.Core.Utils;
using UnityEngine.Tilemaps;

public class GridMonoSystem : MonoBehaviour, IGridMonoSystem
{
	private GameObject _boulderPrefab;

	[SerializeField] private GenerationProperties _properties;
	[SerializeField] private float _cellSize = 1.0f;

	private Tilemap _iceTilemap;
	private Tilemap _stoneTilemap;

	private Tile[,] _iceGrid = new Tile[1,1];
	private Tile[,] _stoneGrid = new Tile[1,1];

	void Start()
	{
		_iceTilemap = GameObject.FindWithTag("IceTilemap").GetComponent<Tilemap>();
		_stoneTilemap = GameObject.FindWithTag("StoneTilemap").GetComponent<Tilemap>();
		_boulderPrefab = Resources.Load<GameObject>("Prefabs/boulder");
		(_iceGrid, _stoneGrid) = new MapGenerator(_properties).collect();
		Spawn();
	}

	public float CellSize() => _cellSize;

	public Tile TileAt(int id, Vector2Int pos)
	{
		Tile[,] grid = GridById(id);
		if (!InBounds(grid, pos)) return null;
		return grid[pos.x, pos.y];
	}

	public bool IsWalkableAt(int id, Vector2Int pos)
	{
		Tile tile = TileAt(id, pos);
		return (
			tile != null &&
			!tile.hasBoulder && (
				tile.type == TileType.Floor ||
				tile.type == TileType.Ice ||
				tile.type == TileType.Start ||
				tile.type == TileType.End ||
				tile.type == TileType.Path
			)
		);
	}

	public void Spawn()
	{
		for (int i = 0; i < 2; i++)
		{
			Tile[,] grid = GridById(i);
			Tilemap tilemap = TilemapById(i);
			for (int x = 0; x < grid.GetLength(0); x++)
			{
				for (int y = 0; y < grid.GetLength(1); y++)
				{
					TileBase tile = _properties.floor;
					if (grid[x, y].type == TileType.Floor) tile = _properties.floor;
					else if (grid[x, y].type == TileType.Hole) tile = _properties.hole;
					else if (grid[x, y].type == TileType.Wall) tile = _properties.wall;
					else if (grid[x, y].type == TileType.Ice) tile = _properties.ice;
					else if (grid[x, y].type == TileType.Water) tile = _properties.water;
					tilemap.SetTile(new Vector3Int(x, y, 0), tile);

					if (grid[x, y].hasBoulder)
					{
						GameObject boulder = GameObject.Instantiate(_boulderPrefab);
						boulder.transform.parent = tilemap.transform;
						boulder.transform.localPosition = new Vector3(
							x * _cellSize, y * _cellSize, -1
						);
						grid[x, y].boulderGameObject = boulder;
					}
				}
			}
		}

	}

	public Transform GridTransform(int id)
	{
		return TilemapById(id).transform;
	}

	private bool InBounds(Tile[,] grid, Vector2Int pos)
	{
		return (
			pos.x >= 0 && pos.x < grid.GetLength(0) &&
			pos.y >= 0 && pos.y < grid.GetLength(1)
		);
	}

	private Tile[,] GridById(int id) {
		if (id == 0) return _iceGrid;
		else return _stoneGrid;
	}

	private Tilemap TilemapById(int id) {
		if (id == 0) return _iceTilemap;
		else return _stoneTilemap;
	}
}
