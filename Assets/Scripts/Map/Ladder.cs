using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private bool up;

    public bool Up
    {
        get => up;
        set => up = value;
    }

    private void Start()
    {
        
        GameManager.Get.AddLadder(this);
    }
}
