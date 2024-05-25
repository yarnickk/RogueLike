using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private Actor actor;

    private void Awake()
    {
        controls = new Controls();
        actor = GetComponent<Actor>();
        if (actor == null)
        {
            Debug.LogError("Actor component is missing on the Player GameObject.");
        }
    }

    private void Start()
    {
        GameManager.Get.Player = actor;
        UpdateCameraPosition();
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Move();
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        // Add functionality if needed
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log($"Move direction: {roundedDirection}");

        GameManager.Action.Move(actor, roundedDirection);
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        }
        else
        {
            Debug.LogError("Main Camera is missing.");
        }
    }
}
