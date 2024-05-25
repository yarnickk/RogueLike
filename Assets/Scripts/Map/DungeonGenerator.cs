using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int maxEnemies = 2; 

    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    private List<Room> rooms = new List<Room>();

    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
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

    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            Room room = new Room(roomX, roomY, roomWidth, roomHeight);

            if (room.Overlaps(rooms))
            {
                continue;
            }

            CreateRoom(room);
            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }

            PlaceEnemies(room, maxEnemies);
            rooms.Add(room);
        }

        GameManager.Get.CreateActor("Player", rooms[0].Center());
    }

    private void CreateRoom(Room room)
    {
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1)
                {
                    TrySetWallTile(pos);
                }
                else
                {
                    SetFloorTile(pos);
                }
            }
        }
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        if (MapManager.Get.FloorMap.GetTile(pos) != null)
        {
            return false;
        }

        MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
        return true;
    }

    private void SetFloorTile(Vector3Int pos)
    {
        if (MapManager.Get.ObstacleMap.GetTile(pos) != null)
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }

        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner = Random.value < 0.5f
                                  ? new Vector2Int(newRoomCenter.x, oldRoomCenter.y)
                                  : new Vector2Int(oldRoomCenter.x, newRoomCenter.y);

        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        foreach (Vector2Int coord in tunnelCoords)
        {
            Vector3Int pos = new Vector3Int(coord.x, coord.y, 0);
            SetFloorTile(pos);

            for (int x = coord.x - 1; x <= coord.x + 1; x++)
            {
                for (int y = coord.y - 1; y <= coord.y + 1; y++)
                {
                    TrySetWallTile(new Vector3Int(x, y, 0));
                }
            }
        }
    }

    private void PlaceEnemies(Room room, int maxEnemies)
    {
        int numEnemies = Random.Range(0, maxEnemies + 1);

        for (int i = 0; i < numEnemies; i++)
        {
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);
            string enemyType = Random.value < 0.5f ? "Ant" : "Tiger";

            GameObject gameObject1 = GameManager.Get.CreateActor(enemyType, new Vector2(x, y));
        }
    }
}
