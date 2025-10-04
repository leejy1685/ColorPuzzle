using UnityEngine;

public enum CellColor 
{
    None = 0,
    Blue = 1,
    Red = 2,
    Yellow = 3,
    Green = 4,
}

public static class ColorConverter
{
    public static Color ColorCodeToColor(CellColor colorCode)
    {
        switch (colorCode)
        {
            case CellColor.Blue:
                return Color.blue;
            case CellColor.Red:
                return Color.red;
            case CellColor.Yellow:
                return Color.yellow;
            case CellColor.Green:
                return Color.green;
        }
        
        //None
        return Color.white;
    }
}
