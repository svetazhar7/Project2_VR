using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelChanger : MonoBehaviour
{
    public string nextSceneName; // Название сцены, на которую нужно переключиться
    private SceneTransition sceneTransition; // Ссылка на компонент SceneTransition

    private void Start()
    {
        // Найти объект SceneTransition в сцене
        sceneTransition = FindObjectOfType<SceneTransition>();

        if (sceneTransition == null)
        {
            Debug.LogError("SceneTransition component not found in the scene!");
        }

        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("Next scene name is not set in the inspector!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player"))
        {
            int yourNumber = FindObjectOfType<PlayerController>().currentLives;
            PlayerPrefs.SetInt("lives", yourNumber);

            if (sceneTransition != null)
            {
                sceneTransition.LoadScene(nextSceneName);
            }
        }

    }
}
