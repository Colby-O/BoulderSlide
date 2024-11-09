using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
	Floor,
	Ice,
	Wall,
	Hole,
	Start,
	End,
	Path
}

public class Tile
{
	public TileType type = TileType.Floor;
	public bool hasBolder = false;
	public GameObject bolderGameObject = null;
}
