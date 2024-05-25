using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Actor Player { get; set; }

    public List<Actor> Enemies { get; private set; } = new List<Actor>();

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

    private void Start()
    {
        // Initialisatiecode indien nodig
    }

    public static GameManager Get => instance;

    public void AddEnemy(Actor enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Attempted to add null enemy to the GameManager.");
            return;
        }

        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Actor enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Attempted to remove null enemy from the GameManager.");
            return;
        }

        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
        }
        else
        {
            Debug.LogError("Attempted to remove enemy that is not in the list.");
        }
    }

    public GameObject CreateActor(string prefabName, Vector3 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{prefabName}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);

        if (actor == null)
        {
            Debug.LogError($"Prefab for {prefabName} could not be loaded.");
            return null;
        }

        if (prefabName == "Player")
        {
            Player = actor.GetComponent<Actor>();
        }
        else if (prefabName != "GravestonePrefabName")
        {
            AddEnemy(actor.GetComponent<Actor>());
        }

        actor.name = prefabName;
        return actor;
    }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        foreach (var enemy in Enemies)
        {
            if (enemy != null && enemy.transform.position == location)
            {
                return enemy;
            }
        }

        return null;
    }

    public void StartEnemyTurn()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.RunAI();
                }
            }
        }
    }
}
