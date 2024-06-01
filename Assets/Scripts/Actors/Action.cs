using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
        
    }

    static public void Move(Actor actor, Vector2 direction)
    {
        actor.Move(direction);
        actor.UpdateFieldOfView();
    }

    static public void Hit(Actor actor, Actor target)
    {
        int damage = actor.Power - target.Defense;

        if (damage > 0)
        {
            target.DoDamage(damage);
            UIManager.Instance.AddMessage($"{actor.name} hits {target.name} for {damage} damage.", actor.GetComponent<Player>() != null ? Color.white : Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{actor.name} hits {target.name} but does no damage.", actor.GetComponent<Player>() != null ? Color.white : Color.red);
        }
    }

    static private void EndTurn(Actor actor)
    {
        if (actor.GetComponent<Player>())
        {
            GameManager.Get.StartEnemyTurn();
        }
    }

}
