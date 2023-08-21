using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    private enum Direction { left = -1, none = 0, right = 1}
    [SerializeField] private Direction dir;
    public int rotMultiplier => (int)dir;
}
