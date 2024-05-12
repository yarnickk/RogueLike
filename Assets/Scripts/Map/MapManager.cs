using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static MapManager Get { get => instance; }

    [Header("TileMaps")]
    public Tilemap FloorMap;
    public Tilemap ObstacleMap;
    public Tilemap FogMap;

    [Header("Features")]
    public Dictionary<Vector2Int, Node> Nodes = new Dictionary<Vector2Int, Node>();


}
