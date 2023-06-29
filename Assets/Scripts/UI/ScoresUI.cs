using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoresUI : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTextsToFade;
    public static ScoresUI Instance;
    public int textToFadeIndex;
    public float moveSpeed;
    public float fadeSpeed;
    public float yMultiplier;
    public Vector3 originalPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        FindScoreTextsToFade();
    }
    private void Start()
    {
        ShootingDiscScript.OnAddBonusPoints += ShowBonusPoints;
        ShootingDiscScript.OnScored += FadeScoreAmount;
    }
    private void FindScoreTextsToFade()
    {
        // Find all TextMeshPro objects under the ScoresUI GameObject and add them to the list
        scoreTextsToFade = GetComponentsInChildren<TextMeshProUGUI>(true);
    }
    private void ShowBonusPoints(object sender, EventArgs e)
    {
        TextMeshProUGUI bonusPointsText = scoreTextsToFade[0];
        bonusPointsText.gameObject.SetActive(true);
        StartCoroutine(Fade(bonusPointsText));
    }
    private void FadeScoreAmount(int scoredAmount)
    {
        textToFadeIndex = scoredAmount;
        TextMeshProUGUI currentTextToFade = scoreTextsToFade[textToFadeIndex];
        currentTextToFade.gameObject.SetActive(true);
        StartCoroutine(Fade(currentTextToFade));
    }
    IEnumerator Fade(TextMeshProUGUI textToDisplay)
    { // Store the initial position and color of the text object
        Vector3 initialPosition = textToDisplay.transform.position;
        Color initialColor = textToDisplay.color;
        // Calculate the target position
        Vector3 targetPosition = initialPosition + (Vector3.up * yMultiplier);
        // Fade and move the text object simultaneously
        while (textToDisplay.transform.position.y < targetPosition.y)
        {
            // Move the text object upwards
            textToDisplay.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // Modify the text object's alpha
            float newAlpha = textToDisplay.color.a - (fadeSpeed * Time.deltaTime);
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);
            textToDisplay.color = newColor;
            yield return null;
        }
        textToDisplay.gameObject.SetActive(false);
        // Reset the text object's position and color
        textToDisplay.transform.position = initialPosition;
        textToDisplay.color = initialColor;
    }
    private void OnDestroy()
    {
        ShootingDiscScript.OnAddBonusPoints -= ShowBonusPoints;
        ShootingDiscScript.OnScored -= FadeScoreAmount;
    }
}