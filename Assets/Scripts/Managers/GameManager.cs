using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GamePaused = false;
    public bool GameOver = false;
    public static GameManager Instance;
    [SerializeField] private UIManager uiManager;
    private int lives = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseHealth()
    {
        if (GameOver)
        {
            lives = 0;
            uiManager.LivesUpdate(lives);
        }
        else
        {
            lives -= 1;
            uiManager.LivesUpdate(lives);
        }
    }
}