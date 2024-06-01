using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	  public void ChangeScene2(string sceneName)
    {
        // Возобновляем время перед загрузкой новой сцены
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
	public void Exit()
	{
		Application.Quit ();
	}
}