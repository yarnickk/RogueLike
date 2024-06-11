using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private int maxEnemies;
    private int maxItems;

    private int currentFloor = 0; 
    private GameObject player;

    List<Room> rooms = new List<Room>();

    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetMaxItems(int max)
    {
        maxItems = max;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }

    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            var room = new Room(roomX, roomY, roomWidth, roomHeight);

            if (room.Overlaps(rooms))
            {
                continue;
            }

            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }
                }
            }

            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }

            PlaceEnemies(room, maxEnemies);
            rooms.Add(room);
        }

        var firstRoom = rooms[0];
        var lastRoom = rooms[rooms.Count - 1];

        PlaceLadder(lastRoom, "down");

        if (player != null)
        {
            MovePlayerToRoom(firstRoom);
        }
        else
        {
            player = GameManager.Get.CreateActor("Player", firstRoom.Center());
        }

        if (currentFloor > 0)
        {
            PlaceLadder(firstRoom, "up");
        }
    }
    public List<Enemy> enemyTypes; // Lijst van alle enemy types

    void Start()
    {
        // Maak een lijst van enemies in volgorde van sterkte
        enemyTypes = new List<Enemy>
        {
            new Enemy("enemy1", 20, 5, 2, 10),
            new Enemy("enemy2", 30, 8, 4, 20),
            new Enemy("enemy3", 40, 12, 6, 30),
            new Enemy("enemy4", 50, 16, 8, 40),
            new Enemy("enemy5", 60, 20, 10, 50),
            new Enemy("enemy6", 70, 24, 12, 60),
            new Enemy("enemy7", 80, 28, 14, 70),
            new Enemy("enemy8", 90, 32, 16, 80)
        };

        PlaceEnemies();
    }
    void PlaceEnemies()
    {
        // Hier plaats je de enemies in de dungeon, met sterkere monsters op diepere niveaus
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            int numEnemies = Random.Range(1, 4); // Random aantal enemies per type
            for (int j = 0; j < numEnemies; j++)
            {
                GameObject enemyPrefab = LoadEnemyPrefab(enemyTypes[i].enemyName);
                Instantiate(enemyPrefab, GetRandomPositionInDungeon(), Quaternion.identity);
            }
        }
    }

    GameObject LoadEnemyPrefab(string enemyName)
    {
        // Laad het prefab voor de gegeven enemy naam
        return Resources.Load<GameObject>("Enemies/" + enemyName);
    }

    Vector3 GetRandomPositionInDungeon()
    {
        // Genereer een willekeurige positie in de dungeon
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }
    private bool TrySetWallTile(Vector3Int pos)
    {
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }

        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    TrySetWallTile(new Vector3Int(x, y, 0));
                }
            }
        }
    }

    private void PlaceEnemies(Room room, int maxEnemies)
    {
        int num = Random.Range(0, maxEnemies + 1);

        for (int counter = 0; counter < num; counter++)
        {
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            if (Random.value < 0.5f)
            {
                GameManager.Get.CreateActor("Ant", new Vector2(x, y));
            }
            else
            {
                GameManager.Get.CreateActor("Tiger", new Vector2(x, y));
            }
        }
    }

    private void PlaceLadder(Room room, string direction)
    {
        if (direction == "down")
        {

            Debug.Log("Ladder down placed at: " + room.Center());

        }
        else if (direction == "up")
        {
            
            Debug.Log("Ladder up placed at: " + room.Center());
            
        }
    }

    private void MovePlayerToRoom(Room room)
    {
        player.transform.position = room.Center();
    }
}
