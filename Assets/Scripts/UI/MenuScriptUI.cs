using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Constants;

public class MenuScriptUI : MonoBehaviour
{
    [SerializeField] RectTransform fader;
    [SerializeField] public static bool multiplayer;
    public static event EventHandler MenuButtonClicked;
    private void Start()
    {
        ActivateFader();
    }
    private void ActivateFader()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() =>
        {
            fader.gameObject.SetActive(false);
        });
    }
    public void IsSinglePlayer()
    {
        multiplayer = false;
    }
    public void IsMultiplayer()
    {
        multiplayer = true;
    }
    public void PlayGame()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
          SceneManager.LoadScene(AppConstants.menuSceneName, LoadSceneMode.Single);
        });
    }
    public void GoToMenu()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
        MenuButtonClicked?.Invoke(this, EventArgs.Empty);
    }
    public void LoadRules()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            SceneManager.LoadScene(2);
        });
    }
    public void QuitGame()
    {
        AudioListener.volume = 0;
        Debug.Log("Quit");
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Application.Quit();
        });
    }
}