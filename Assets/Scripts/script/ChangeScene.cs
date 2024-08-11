using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public GameObject panel;
    public float load = 0;
    public Slider loadslider;
    public Text loadtext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(load >= 100)
        {
            SceneManager.LoadScene("Level0");
        }
    }
    public void play()
    {
        panel.SetActive(true);
        StartCoroutine(loadingbar());
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    IEnumerator loadingbar()
    {
        while(load < 100)
        {
            load += 1f;
            loadslider.value = load;
            loadtext.text = "Loading " + load + "%";
            yield return new WaitForSeconds(0.05f);
        }
    }
}
