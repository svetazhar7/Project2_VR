using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    public string[] scenesToMute;

    private static PersistentAudio instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Сохраняем единственный экземпляр объекта
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // Подписываемся на событие смены сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Отписываемся от события смены сцены
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, нужно ли выключать аудио в текущей сцене
        if (System.Array.Exists(scenesToMute, element => element == scene.name))
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}
