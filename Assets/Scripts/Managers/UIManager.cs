using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    private VolumeSettings volumeSettings;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesCounter;
    [SerializeField] private CoinManager coinManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        volumeSettings = FindObjectOfType<VolumeSettings>();
        ShowDeathScreen(false);
    }

    public void ScoreUpdate(float score)
    {
        scoreText.text = Mathf.Round(score).ToString();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        volumeSettings.SaveVolume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ShowDeathScreen(false);
    }

    public void ShowDeathScreen(bool showMenu)
    {
        deathScreen.SetActive(showMenu);
        coinManager.OnGameEnd();
    }

    public void PauseButton()
    {
        bool paused = gameManager.GamePaused;
        if (paused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        gameManager.GamePaused = !paused;
        pauseMenu.SetActive(!paused);
    }

    public void LivesUpdate(int lives)
    {
        livesCounter.text = lives.ToString();
    }
}