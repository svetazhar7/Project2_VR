using UnityEngine;

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

            if (sceneTransition != null)
            {
                sceneTransition.LoadScene(nextSceneName);
            }
        }

    }
}
