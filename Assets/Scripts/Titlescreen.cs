using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlescreen : MonoBehaviour
{
    public GameObject options;
    public GameObject credits;
    public GameObject title;
    public Animator an;
    private void Start()
    {
        an.Play("title");
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
    public void Options()
    {
        print("opened options i guess");
        options.SetActive(true);
        title.SetActive(false);
    }
    public void Credits()
    {
        print("opened credits i guess");
        credits.SetActive(true);
        title.SetActive(false);
    }

    public void Back()
    {
        options.SetActive(false);
        credits.SetActive(false);
        title.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
