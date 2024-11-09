using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GenerationProperties", menuName = "Generation/New Properties")]
public class GenerationProperties : ScriptableObject
{
    [Header("Properties")]
    public Vector2Int size;
    public int minNumBoulders;
    public float turnProb;

    [Header("Sprites")]
    public TileBase wall;
    public TileBase floor;
    public TileBase hole;
}
