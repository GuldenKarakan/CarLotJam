using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private Transform parent;
    int levelIndex = 1;
    GameObject level;

    private void Awake()
    {
        Next();
    }
    public void Next()
    {
        if (parent.childCount != 0)
            DestroyImmediate(parent.GetChild(0));

        level = Instantiate(Resources.Load<GameObject>("levels/level-" + levelIndex), parent).GetComponent<GameObject>();
        levelIndex++;
        if (levelIndex < 2)
            levelIndex = 0;

    }
}
