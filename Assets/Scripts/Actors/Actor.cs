using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private AdamMilVisibility algorithm;
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();
    public int FieldOfViewRange = 8;

    private void Start()
    {
        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
    }

    public void Move(Vector3 direction)
    {
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction;
        }
    }

    public void UpdateFieldOfView()
    {
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        FieldOfView.Clear();
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }
}
