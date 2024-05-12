using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    static public void Move(Actor actor, Vector2 direction)
    {
        actor.Move(direction);
        actor.UpdateFieldOfView();
    }
}
