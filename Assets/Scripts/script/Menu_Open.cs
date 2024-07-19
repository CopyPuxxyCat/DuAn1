using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Open : MonoBehaviour
{
    public GameObject PauseGame;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            PauseGame.SetActive(true);
        }
    }
}
