using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject player;
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField]
    private GameObject faildPlayer;
    [SerializeField] private GameObject hearth;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject deathButtons;
    [SerializeField]
    private GameObject environments;
    [SerializeField]
    private GameObject trophay;

    public void DeadState()
    {
        GameManager.Instance.GameOver = true;
        GameManager.Instance.DecreaseHealth();
        SaveScore();
        uiManager.ShowDeathScreen(true);
        playerMovement.enabled = false;
        faildPlayer.SetActive(true);
        hearth.SetActive(false);
        pause.SetActive(false);
        deathButtons.SetActive(true);
        environments.SetActive(false);
        trophay.SetActive(true);
    }

    private void SaveScore()
    {
        if (coinManager != null && SaveManager.Instance != null && SaveManager.Instance.SaveDataArray != null && SaveManager.Instance.SaveDataArray.saves.Count > 0)
        {
            if (coinManager.CoinCount > SaveManager.Instance.SaveDataArray.saves[^1].score)
            {
                SaveManager.Instance.SaveDataArray.saves[^1].score = (int)coinManager.CoinCount;
                SaveManager.Instance.SaveData();
            }
        }
        else
        {
            // Handle the case where any of the required objects is null.
            Debug.LogWarning("Unable to save score. Check if coinManager, SaveManager.Instance, or SaveDataArray is null.");
        }
    }
}