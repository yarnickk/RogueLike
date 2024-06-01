using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Enemy : MonoBehaviour
{
    public Actor Target;
    public bool IsFighting = false;
    private AStar algorithm;
    public int confused = 0;
    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
        algorithm = GetComponent<AStar>();
    }

    public void MoveAlongPath(Vector3Int targetPosition)
    {
        Vector3Int gridPosition = MapManager.Get.FloorMap.WorldToCell(transform.position);
        Vector2 direction = algorithm.Compute((Vector2Int)gridPosition, (Vector2Int)targetPosition);
        Action.MoveOrHit(GetComponent<Actor>(), direction);
    }
    public void Confuse()
    {
        confused = 8;
    }

    public void RunAI()
    {
        if (Target == null)
        {
            Target = GameManager.Get.Player;
        }

        Vector3 targetPosition = Target.transform.position;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget < 1.5f)
        {

            Action.Hit(GetComponent<Actor>(), Target);
            IsFighting = true;
        }
        else
        {

            var gridPosition = MapManager.Get.FloorMap.WorldToCell(targetPosition);
            MoveAlongPath(gridPosition);
        }

        if (confused > 0)
        {
            Debug.Log("The enemy is confused and cannot act");
            confused--;
            return;
        }
    }
}