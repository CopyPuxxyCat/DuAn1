using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{
    public GameObject PauseGame, Panel;
    public Player_Controller p1;

    void Start()
    {
        p1 = FindObjectOfType<Player_Controller>();
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

    public void loi2()
    {
        p1.Hthoai1.SetActive(false);
        p1.Hthoai2.SetActive(true);
        p1.Hthoai3.SetActive(false);
    }
    public void loi3()
    {
        p1.Hthoai1.SetActive(false);
        p1.Hthoai2.SetActive(false);
        p1.Hthoai3.SetActive(true);
    }
    public void Ketthuc()
    {
        p1.Panel_giaotiep.SetActive(false);
    }
}
