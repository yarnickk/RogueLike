using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null. Make sure it exists in the scene.");
            }
            return _instance;
        }
    }

    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    public Actor Player { get; private set; }
    public List<Actor> Enemies { get; private set; } = new List<Actor>();
    public List<Consumable> Items { get; private set; } = new List<Consumable>();
    public List<Ladder> Ladders { get; private set; } = new List<Ladder>();
    public List<Tombstone> Tombstones { get; private set; } = new List<Tombstone>();

    
    public GameObject CreateActor(string name, Vector2 position)
    {
        return InstantiatePrefab($"Prefabs/{name}", position, $"Failed to create actor: {name}");
    }

    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
        UpdateEnemiesLeftText();
    }

    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Remove(enemy))
        {
            Destroy(enemy.gameObject);
            UpdateEnemiesLeftText();
        }
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        return Enemies.Find(enemy => enemy.transform.position == location);
    }

    
    public GameObject CreateItem(string itemName, Vector2 position)
    {
        return InstantiatePrefab($"Prefabs/{itemName}", position, $"Failed to create item: {itemName}");
    }

    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }

    public void RemoveItem(Consumable item)
    {
        if (Items.Remove(item))
        {
            Destroy(item.gameObject);
        }
    }

    public Consumable GetItemAtLocation(Vector3 location)
    {
        return Items.Find(item => item.transform.position == location);
    }

    
    public void CreateLadder(Vector2 position, bool up)
    {
        GameObject ladderObject = InstantiatePrefab("Prefabs/Ladder", position, "Failed to load ladder prefab");
        if (ladderObject != null)
        {
            Ladder ladder = ladderObject.GetComponent<Ladder>();
            if (ladder != null)
            {
                ladder.Up = up;
                Ladders.Add(ladder);
            }
            else
            {
                Debug.LogError("Failed to create ladder: Ladder component is missing.");
                Destroy(ladderObject);
            }
        }
    }

    public void AddLadder(Ladder ladder)
    {
        Ladders.Add(ladder);
    }

    public Ladder GetLadderAtLocation(Vector3 location)
    {
        return Ladders.Find(ladder => ladder.transform.position == location);
    }

    
    public void AddTombstone(Tombstone stone)
    {
        Tombstones.Add(stone);
    }

    
    public void MoveActorToPosition(Actor actor, Vector2 position)
    {
        if (actor != null)
        {
            actor.transform.position = new Vector3(position.x, position.y, 0);
        }
        else
        {
            Debug.LogError("Failed to move actor: Actor is null.");
        }
    }

    
    public void StartEnemyTurn()
    {
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<Enemy>().RunAI();
        }
    }

    
    public List<Actor> GetNearbyEnemies(Vector3 location, float radius = 5f)
    {
        return Enemies.FindAll(enemy => Vector3.Distance(enemy.transform.position, location) < radius);
    }

    
    [Serializable]
    public struct SaveGame
    {
        public int maxHitPoints;
        public int hitPoints;
        public int defense;
        public int power;
        public int level;
        public int xp;
        public int xpToNextLevel;
    }

    private SaveGame _playerSaveData;

    public void SavePlayerData()
    {
        _playerSaveData = new SaveGame
        {
            maxHitPoints = Player.MaxHitPoints,
            hitPoints = Player.HitPoints,
            defense = Player.Defense,
            power = Player.Power,
            level = Player.Level,
            xp = Player.XP,
            xpToNextLevel = Player.XPToNextLevel
        };

        string jsonData = JsonUtility.ToJson(_playerSaveData);
        PlayerPrefs.SetString("PlayerSaveData", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerSaveData"))
        {
            string jsonData = PlayerPrefs.GetString("PlayerSaveData");
            _playerSaveData = JsonUtility.FromJson<SaveGame>(jsonData);

            Player.MaxHitPoints = _playerSaveData.maxHitPoints;
            Player.HitPoints = _playerSaveData.hitPoints;
            Player.Defense = _playerSaveData.defense;
            Player.Power = _playerSaveData.power;
            Player.Level = _playerSaveData.level;
            Player.XP = _playerSaveData.xp;
            Player.XPToNextLevel = _playerSaveData.xpToNextLevel;
        }
    }

    public void RemoveSaveGame()
    {
        PlayerPrefs.DeleteKey("PlayerSaveData");
        PlayerPrefs.Save();
    }

    
    public void ClearFloor()
    {
        ClearList(Enemies);
        ClearList(Items);
        ClearList(Ladders);
        ClearList(Tombstones);
        UpdateEnemiesLeftText();
    }

    
    private GameObject InstantiatePrefab(string path, Vector2 position, string errorMessage)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError(errorMessage);
            return null;
        }
        return Instantiate(prefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
    }

    private void UpdateEnemiesLeftText()
    {
        UIManager.Instance.UpdateEnemiesLeftText(Enemies.Count);
    }

    private void ClearList<T>(List<T> list) where T : MonoBehaviour
    {
        foreach (var item in list)
        {
            Destroy(item.gameObject);
        }
        list.Clear();
    }
}
