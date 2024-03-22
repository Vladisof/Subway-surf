using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject leaderboardMenu;
    [SerializeField] private TextMeshProUGUI usernameInput;

    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SubmitButton()
    {
        if (SaveManager.Instance.SaveDataArray.saves.Count > 0)
        {
            SaveManager.Instance.SaveDataArray.saves[^1].username = usernameInput.text;
        }
    }

    public void OptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void LeaderboardButton()
    {
        mainMenu.SetActive(false);
        leaderboardMenu.SetActive(true);
        SaveManager.Instance.CreateLeaderboard();
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        leaderboardMenu.SetActive(false);
    }
}