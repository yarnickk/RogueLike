using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Consumable> Items { get; private set; } = new List<Consumable>();
    public int MaxItems;

    public bool AddItem(Consumable item)
    {
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            return true;
        }
        return false;
    }

    public void DropItem(Consumable item)
    {
        Items.Remove(item);
    }
}
