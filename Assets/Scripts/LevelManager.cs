using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform parent;
    [SerializeField] private ParticleSystem finishParticle;
    int levelIndex = 1;
    Level level;
    bool isFinish = false;

    private void Awake()
    {
        Next();
    }

    private void Update()
    {
        if (isFinish) return;

        if (level.isFinish)
        {
            isFinish = true;
            panel.SetActive(true);
            finishParticle.Play();
        }
    }
    public void Next()
    {
        isFinish = false;
        if (parent.childCount != 0)
            Destroy(parent.GetChild(0).gameObject);

        level = Instantiate(Resources.Load<GameObject>("levels/level-" + levelIndex), parent).GetComponent<Level>();
        CameraControl.instance.CameraMode();
        levelIndex++;
        if (levelIndex == 3)
            levelIndex = 1;
        panel.SetActive(false);

    }
}
