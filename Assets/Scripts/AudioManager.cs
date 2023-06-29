using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private static AudioSource audioSource;
    [SerializeField] private AudioClip shootingSoundclip, ScoreSoundClip, bonusPointSoundClip, buttonClickSoundClip, gameOverSoundClip, placeDiscSound;
    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        ShootingDiscScript.OnAddBonusPoints += ShootingDiscScript_OnAddBonusPoints;
        ShootingDiscScript.OnScored += ShootingDiscScript_OnScored;
        ShootingDiscScript.OnShooting += ShootingDiscScript_OnShooting;
        GameManager.GameOver += GameManager_OnGameOver;
        AimingDiscScript.OnDiscPlaced += AimingDiscScript_OnDiscPlaced;
    }
    private void AimingDiscScript_OnDiscPlaced(object sender, System.EventArgs e)
    {
        audioSource.clip = placeDiscSound;
        audioSource.Play();
    }
    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        audioSource.clip = gameOverSoundClip;
        audioSource.Play();
    }
    private void ShootingDiscScript_OnShooting(object sender, System.EventArgs e)
    {
        audioSource.clip = shootingSoundclip;
        audioSource.Play();
    }
    private void ShootingDiscScript_OnScored(int scoreAmount)
    {
        audioSource.clip = ScoreSoundClip;
        audioSource.Play();
    }
    private void ShootingDiscScript_OnAddBonusPoints(object sender, System.EventArgs e)
    {

        audioSource.clip = bonusPointSoundClip;
        audioSource.Play();
    }
    public void PlayClickButtonSound()
    {
        audioSource.clip = buttonClickSoundClip;
        audioSource.Play();
    }
    private void OnDestroy()
    {
        ShootingDiscScript.OnAddBonusPoints -= ShootingDiscScript_OnAddBonusPoints;
        ShootingDiscScript.OnScored -= ShootingDiscScript_OnScored;
        ShootingDiscScript.OnShooting -= ShootingDiscScript_OnShooting;
        GameManager.GameOver -= GameManager_OnGameOver;
        AimingDiscScript.OnDiscPlaced -= AimingDiscScript_OnDiscPlaced;
    }
}
