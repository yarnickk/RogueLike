using UnityEngine;

public class Tombstone : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddTombstone(this);
    }
}
