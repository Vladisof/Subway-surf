using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderDeep : MonoBehaviour
{
  // Имя целевой сцены
  public string targetSceneName;

  // Метод для перехода на другую сцену
  public void LoadTargetScene()
  {
    PlayerPrefs.SetInt("VisitedMenuBonus", 1);
    SceneManager.LoadScene(targetSceneName);
  }
  
}