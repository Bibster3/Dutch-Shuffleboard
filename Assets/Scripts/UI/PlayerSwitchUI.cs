using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSwitchUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerSwitchText;
    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    public static PlayerSwitchUI instance;
    public float duration = 3f;
    public Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);

    public static PlayerSwitchUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerSwitchUI>();
                if (instance == null)
                {
                    PlayerSwitchUI go = new PlayerSwitchUI();
                    go.name = "PlayerSwitchUI";
                    instance = go.AddComponent<PlayerSwitchUI>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           DontDestroyOnLoad(this.gameObject); //<<1>> If I comment out this line, the problem does not occur[/B]
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (instance ==null)
        {
            instance = FindObjectOfType<PlayerSwitchUI>();
        }
        playerSwitchText.gameObject.SetActive(false);
        GameManager.UIPlayerTurnStart += GameManager_OnPlayerTurnStart;
    }
    private void GameManager_OnPlayerTurnStart(object sender, EventArgs e)
    {
        if (GameManager.IsMultiplayer())
        {
            Debug.Log("CoroutineCalled");
            StartCoroutine(ShowPlayerTurn());
            GameManager.isUIPlayerTurnInvoked = true; 
        }
        if (GameManager.State == GameManager.GameState.PlayerTwoTurn)
        {
            playerOneScoreText.text = "Player 1 Score:" + GameManager.playerOneScore;
            playerOneScoreText.gameObject.SetActive(true);
        }
    }
    private IEnumerator ShowPlayerTurn()
    {
        playerSwitchText.gameObject.SetActive(true);
        Debug.Log("PlayerSwitchActivated");

        if (GameManager.State == GameManager.GameState.PlayerOneTurn)
        {
            playerSwitchText.text = "Player 1 Turn";
        }
        else if (GameManager.State == GameManager.GameState.PlayerTwoTurn)
        {
            playerSwitchText.text = "Player 2 Turn";
        }
        Vector3 initialScale = playerSwitchText.transform.localScale;
        Color initialColor = playerSwitchText.color;
        // Set the initial scale and alpha values for appearance
        playerSwitchText.transform.localScale = Vector3.zero;
        Color transparentColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        playerSwitchText.color = transparentColor;
        playerSwitchText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // Animate the disappearance
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // Scale down the text object using Lerp
            float scaleProgress = timer / duration;
            playerSwitchText.transform.localScale = Vector3.Lerp(initialScale, targetScale, scaleProgress);
            // Fade out the text object using Lerp
            float alphaProgress = timer / duration;
            Color lerpedColor = Color.Lerp(initialColor, transparentColor, alphaProgress);
            playerSwitchText.color = lerpedColor;
            yield return null;
        }
        playerSwitchText.gameObject.SetActive(false);
        playerSwitchText.transform.localScale = initialScale;
        playerSwitchText.color = initialColor;
    }
    private void OnDestroy()
    {
        GameManager.UIPlayerTurnStart -= GameManager_OnPlayerTurnStart;
    }
}