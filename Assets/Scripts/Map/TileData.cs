using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData 
{
    public string Name;
    public bool IsExplored;
    public bool IsVisible;

    public TileData(string name, bool isExplored, bool isVisible)
    {
        Name = name;
        IsExplored = isExplored;
        IsVisible = isVisible;
    }
}
