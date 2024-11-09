using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlazmaGames.Core.MonoSystem;

public interface IGridMonoSystem : IMonoSystem
{
	public float CellSize();
	public Tile TileAt(int id, Vector2Int pos);
	public bool IsWalkableAt(int id, Vector2Int pos);
	public void Spawn();
	public Transform GridTransform(int id);
}
