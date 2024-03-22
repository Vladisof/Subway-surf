using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  // Имя целевой сцены
  public string targetSceneName;

  // Метод для перехода на другую сцену
  public void LoadTargetScene()
  {
    // Загружаем сцену по её имени
    SceneManager.LoadScene(targetSceneName);
  }

  // Метод для выхода из игры
  public void ExitGame()
  {
    // Этот код работает как в редакторе Unity, так и при сборке игры
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }
}