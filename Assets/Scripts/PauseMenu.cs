using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public AudioSource audioSource; // Ссылка на AudioSource, который нужно приостанавливать
    private bool isPaused = false;

    void Start()
    {
        // Панель паузы изначально неактивна
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Проверяем, была ли нажата клавиша Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Переключаем состояние паузы
            isPaused = !isPaused;
            // Если игра была поставлена на паузу
            if (isPaused)
            {
                // Останавливаем время в игре
                Time.timeScale = 0f;
                // Активируем панель паузы
                pauseMenuUI.SetActive(true);
                // Приостанавливаем звук
                audioSource.Pause();
            }
            else
            {
                // Возобновляем время в игре
                Time.timeScale = 1f;
                // Деактивируем панель паузы
                pauseMenuUI.SetActive(false);
                // Возобновляем звук
                audioSource.UnPause();
            }
        }
    }

    public void ResumeGame()
    {
        // Выходим из режима паузы
        isPaused = false;
        // Возобновляем время в игре
        Time.timeScale = 1f;
        // Деактивируем панель паузы
        pauseMenuUI.SetActive(false);
        // Возобновляем звук
        audioSource.UnPause();
    }
}