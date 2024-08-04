using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    public GameObject PauseGame, Panel;

    void Update()
    {

    }
    public void back()
    {
        Time.timeScale = 1f;
        Panel.SetActive(false);
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
        PauseGame.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }
    public void Menu_return()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
