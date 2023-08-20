using UnityEngine;

[System.Serializable]
public class CustomColor
{
    public Colors colorName;
    public Material colorMaterial;
}

[System.Serializable]
public enum Colors : int
{
    none = 0,
    purple = 1,
    black = 2,
    blue = 3,
    green = 4,
    orange = 5,
    pink = 6,
    red = 7,
    yellow = 8,
}
