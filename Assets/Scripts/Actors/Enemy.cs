using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target { get; set; }
    public bool IsFighting { get; private set; } = false;
    private AStar algorithm;

    private void Start()
    {
        algorithm = GetComponent<AStar>();
        if (algorithm == null)
        {
            Debug.LogError($"{gameObject.name}: AStar component is missing.");
        }

        Actor actor = GetComponent<Actor>();
        if (actor == null)
        {
            Debug.LogError($"{gameObject.name}: Actor component is missing.");
            return;
        }

        GameManager.Get.AddEnemy(actor);
    }

    private void Update()
    {
        RunAI();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        if (algorithm == null)
        {
            Debug.LogError($"{gameObject.name}: Algorithm is not initialized.");
            return;
        }

        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);

        if (direction == Vector2.zero)
        {
            Debug.LogError($"{gameObject.name}: Algorithm.Compute returned zero direction.");
            return;
        }

        Actor actor = GetComponent<Actor>();
        if (actor == null)
        {
            Debug.LogError($"{gameObject.name}: Actor component is missing.");
            return;
        }

        Action.Move(actor, direction);
    }

    public void RunAI()
    {
        if (Target == null)
        {
            Target = GameManager.Get.Player;
            if (Target == null)
            {
                Debug.LogError($"{gameObject.name}: Target is null, GameManager.Get.Player is not assigned.");
                return;
            }
        }

        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(Target.transform.position);

        Actor actor = GetComponent<Actor>();
        if (actor == null)
        {
            Debug.LogError($"{gameObject.name}: Actor component is missing.");
            return;
        }

        if (IsFighting || actor.FieldOfView.Contains(gridPosition))
        {
            IsFighting = true;
            MoveAlongPath(gridPosition);
        }
    }
}
