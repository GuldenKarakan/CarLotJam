using UnityEngine;

[System.Serializable]
public class CustomColor
{
    public Colors colorName;
}

[System.Serializable]
public enum Colors : int
{
    none = 0,
    purple = 1,
    pink = 2,
    green = 3,
    orange = 4,
    blue = 5,
    yellow = 6,
    red = 7
}
