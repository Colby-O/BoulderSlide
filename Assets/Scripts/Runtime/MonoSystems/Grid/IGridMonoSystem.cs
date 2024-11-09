using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core.MonoSystem;
using UnityEngine.Tilemaps;

public interface IGridMonoSystem : IMonoSystem
{
	public float CellSize();
	public Tile TileAt(int id, Vector2Int pos);
    public Tilemap GetTilemap(int id);
	public void SetTileAt(int id, Vector2Int pos, TileType type);
    public bool IsWalkableAt(int id, Vector2Int pos);
	public void Spawn();
	public Transform GridTransform(int id);
	public void NewGrid(int numHoles);

    public void ResetGrid();
}
