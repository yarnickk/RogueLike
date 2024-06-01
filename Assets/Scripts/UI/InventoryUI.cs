using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public Label[] labels = new Label[8];
    public VisualElement root;

    private int selected;
    private int numItems;

    public int Selected { get { return selected; } }

    public void Clear()
    {
        foreach (var label in labels)
        {
            label.text = "";
        }
    }

    private void UpdateSelected()
    {
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
                labels[i].style.backgroundColor = Color.green;
            else
                labels[i].style.backgroundColor = Color.clear;
        }
    }

    public void SelectNextItem()
    {
        selected = Mathf.Min(selected + 1, numItems - 1);
        UpdateSelected();
    }

    public void SelectPreviousItem()
    {
        selected = Mathf.Max(selected - 1, 0);
        UpdateSelected();
    }

    public void Show(List<Consumable> list)
    {
        selected = 0;
        numItems = list.Count;
        Clear();
        for (int i = 0; i < numItems; i++)
        {
            labels[i].text = list[i].name;
        }
        UpdateSelected();
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
}