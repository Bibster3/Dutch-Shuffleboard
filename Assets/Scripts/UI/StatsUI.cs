
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public Text hitText;
    public Text scoreText;
    public Text subTurns;
    public static StatsUI Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        scoreText.text = "Score:" + GameManager.Score;
        hitText.text = "Hits Left:" + GameManager.Hits;
        subTurns.text = "Turns:" + GameManager.SubTurns;
    }
}
