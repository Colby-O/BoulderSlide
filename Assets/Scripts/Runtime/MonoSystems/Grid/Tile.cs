using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
	None,
	Ice,
	Wall,
	Water,
	Hole,
	Start,
	End,
	Floor,
	Path,
	Push,
	Boulder,
	HoleFilled
}

public class Tile
{
	public TileType type = TileType.Floor;
	public bool hasBoulder = false;
	public GameObject boulderGameObject = null;

	public Tile(TileType type)
	{
		this.type = type;
	}
}
