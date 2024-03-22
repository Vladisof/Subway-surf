using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
  public TextMeshProUGUI maxScoreText;
  public TextMeshProUGUI lastScoreText;

  private void Start()
  {
    UpdateUI();
  }

  public void UpdateUI()
  {
    // Загружаем максимальный рекорд из PlayerPrefs
    float maxScore = PlayerPrefs.GetFloat("MaxScore", 0f);
    maxScoreText.text = "Best Score: " + Mathf.Round(maxScore).ToString();

    // Загружаем последний счет из PlayerPrefs
    float lastScore = PlayerPrefs.GetFloat("LastScore", 0f);
    lastScoreText.text = "Last Score: " + Mathf.Round(lastScore).ToString();
  }

  public void SaveScore(float currentScore)
  {
    // Загружаем максимальный рекорд из PlayerPrefs
    float maxScore = PlayerPrefs.GetFloat("MaxScore", 0f);

    // Если текущий счет больше максимального, обновляем максимальный рекорд
    if (currentScore > maxScore)
    {
      maxScore = currentScore;
      PlayerPrefs.SetFloat("MaxScore", maxScore);
    }

    // Сохраняем текущий счет как последний счет
    PlayerPrefs.SetFloat("LastScore", currentScore);

    // Сохраняем изменения в PlayerPrefs
    PlayerPrefs.Save();

    // Обновляем UI
    UpdateUI();
  }
}