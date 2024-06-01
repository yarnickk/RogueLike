using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Inventory inventory;
    public void OnGrab(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Implement grab action
    }

    public void OnDrop(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Implement drop action
    }

    public void OnUse(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Implement use action
    }

    public void OnSelect(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Implement select action
    }

    public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Move();
        }
    }

    public void OnExit(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Implement exit action
    }

    private void UseItem(Consumable item)
    {
        // Implement use item logic
    }

    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();
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
    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }
}