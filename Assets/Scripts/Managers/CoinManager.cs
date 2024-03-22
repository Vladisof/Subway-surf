using UnityEngine;

public class CoinManager : MonoBehaviour
{
  public float CoinCount;
  public UIManager UIManager;
  [SerializeField] private float scorePerSecond = 1f;

  private void Update()
  {
    if(GameManager.Instance.GameOver)
    {
      return;
    }
    if (!GameManager.Instance.GamePaused)
    {
      CoinCount += scorePerSecond * Time.deltaTime;
    }
    UIManager.ScoreUpdate(CoinCount);
  }

  // Вызывается при завершении игры
  public void OnGameEnd()
  {
    // Сохраняем текущий счет в PlayerPrefs
    PlayerPrefs.SetFloat("LastScore", CoinCount);

    // Загружаем максимальный рекорд из PlayerPrefs
    float maxScore = PlayerPrefs.GetFloat("MaxScore", 0f);

    // Если текущий счет больше максимального, обновляем максимальный рекорд
    if (CoinCount > maxScore)
    {
      maxScore = CoinCount;
      PlayerPrefs.SetFloat("MaxScore", maxScore);
    }

    // Сохраняем изменения в PlayerPrefs
    PlayerPrefs.Save();
  }
}