using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuScript : MonoBehaviour
{
    [SerializeField] RectTransform fader;

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

    public void GoToMenu()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
