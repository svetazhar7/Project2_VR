using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // Ссылка на UI Image для затемнения
    public float fadeDuration = 1f; // Продолжительность затемнения

    private void Start()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("Fade Image is not set in the inspector!");
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f); // Убедитесь, что Alpha установлен в 0
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 1f); // Убедитесь, что Alpha установлен в 1

        SceneManager.LoadScene(sceneName);
    }
}
