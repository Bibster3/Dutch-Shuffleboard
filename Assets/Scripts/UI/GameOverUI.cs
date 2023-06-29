using UnityEngine;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] Text finalResultsText;
    public static GameOverUI Instance;
    public Text highScoreText;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        HideGameOver();
    }
    public void HideGameOver()
    {
        gameOverMenu.gameObject.SetActive(false);
    }
    public void ShowGameOver()
    {
        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");
        gameOverMenu.gameObject.SetActive(true);
        if (GameManager.IsMultiplayer())
        {
            {
                if (GameManager.playerOneScore > GameManager.playerTwoScore)
                {
                    finalResultsText.text = "PLAYER 1 WINS!";
                    Debug.Log("PLAYER 1 WINS");
                }
                else if (GameManager.playerOneScore < GameManager.playerTwoScore)
                {
                    finalResultsText.text = "PLAYER 2 WINS!";
                    Debug.Log("PLAYER 2 WINS");
                }
                else
                {
                    finalResultsText.text = "It's a TIE!";
                    Debug.Log("It's a TIE!");
                }
            }
        }
        else if (!GameManager.IsMultiplayer())
        {
            finalResultsText.text = "Your final score is " + GameManager.Score;
        }
    }
}