using Constants;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event EventHandler DiscResetForAnotherTurn;
    public static event EventHandler SwitchToNextPlayer;
    public static event EventHandler UIPlayerTurnStart;
    public static event EventHandler GameOver;
    public static GameState State;
    public static int Hits = AppConstants.InitHitsCount;
    public static int Score = 0;
    public static int SubTurns = 3;
    public static int discsToReshoot = AppConstants.InitHitsCount;
    public static int playerOneScore;
    public static int playerTwoScore;
    public static bool isUIPlayerTurnInvoked = false;
    private bool isGameResetForSecondPlayer = false;
    public enum GameState
    {
        PlayerOneTurn,
        PlayerTwoTurn,
        GameOver
    }
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
        State = GameState.PlayerOneTurn;
        Debug.Log("IsMultiplayer" + IsMultiplayer());
        ShootingDiscScript.OnAddBonusPoints += GameManager_OnAddBonusPoints;
        MenuScriptUI.MenuButtonClicked += MenuScriptUI_MenuButtonClicked;
    }
    private void Update()
    {
        switch (State)
        {
            case GameState.PlayerOneTurn:
                PlayerOneTurn();
                break;
            case GameState.PlayerTwoTurn:
                PlayerTwoTurn();
                break;
            case GameState.GameOver:
                GameOver?.Invoke(this, EventArgs.Empty);
                GameOverUI.Instance.ShowGameOver();
                break;
            default:
                return;
        }
    }
    private void ResetStatsForPlayer()
    {
        Hits = AppConstants.InitHitsCount;
        Score = 0;
        SubTurns = 3;
        discsToReshoot = AppConstants.InitHitsCount;
        Debug.Log("playerOneScore" + playerOneScore + "Hits" + Hits + " Score" + Score + " SubTurns" + SubTurns + " DiscsToReshoot" + discsToReshoot);
    }
    private void PlayerOneTurn()
    {
        if ((IsMultiplayer()) && (!isUIPlayerTurnInvoked))
        {

            UIPlayerTurnStart?.Invoke(this, EventArgs.Empty);
            isUIPlayerTurnInvoked = true;
        }
        if (Hits == 0)
        {
            SubTurns--;
            if (SubTurns > 0)
            {
                DiscResetForAnotherTurn?.Invoke(this, EventArgs.Empty);
                ResetHitsForOneMoreTurn();
            }
            else if (SubTurns == 0)
            {
                SetHighScore();
                SavePlayerOneScore();
                if (IsMultiplayer())
                {
                    isUIPlayerTurnInvoked = false;
                    State = GameState.PlayerTwoTurn;
                }
                else
                {
                    State = GameState.GameOver;
                }
            }
        }
    }
    private void PlayerTwoTurn()
    {
        if (!isUIPlayerTurnInvoked)
        {
            UIPlayerTurnStart?.Invoke(this, EventArgs.Empty);
            isUIPlayerTurnInvoked = true;
        }
        if (!isGameResetForSecondPlayer)
        {
            ResetStatsForPlayer();
            SwitchToNextPlayer?.Invoke(this, EventArgs.Empty);
            isGameResetForSecondPlayer = true;
        }
        if (Hits == 0)
        {
            SubTurns--;
            if (SubTurns > 0)
            {
                DiscResetForAnotherTurn?.Invoke(this, EventArgs.Empty);
                ResetHitsForOneMoreTurn();
            }
            else if (SubTurns == 0)
            {
                SetHighScore();
                SavePlayerTwoScore();
                State = GameState.GameOver;
            }
        }
    }
    private void MenuScriptUI_MenuButtonClicked(object sender, EventArgs e)
    {
        ResetStatsForPlayer();
        isGameResetForSecondPlayer = false;
        isUIPlayerTurnInvoked = false;
        playerOneScore = 0;
        playerTwoScore = 0;
        State = GameState.PlayerOneTurn;
    }

    private void SavePlayerOneScore()
    {
        playerOneScore = Score;
    }
    private void SavePlayerTwoScore()
    {
        playerTwoScore = Score;
    }
    private void ResetHitsForOneMoreTurn()
    {
        Hits = discsToReshoot;
    }
    void SetHighScore()
    {
        if (Score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", Score);
            GameOverUI.Instance.highScoreText.text = "NEW HIGHSCORE" + PlayerPrefs.GetInt("highscore");
        }
    }
    public static bool IsMultiplayer()
    {
        return MenuScriptUI.multiplayer;
    }
    private void GameManager_OnAddBonusPoints(object sender, EventArgs e)
    {
        Score = Score + 10;
    }
    private void OnDestroy()
    {
        ShootingDiscScript.OnAddBonusPoints -= GameManager_OnAddBonusPoints;
        MenuScriptUI.MenuButtonClicked -= MenuScriptUI_MenuButtonClicked;
    }
}