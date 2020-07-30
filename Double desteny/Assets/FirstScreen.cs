using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstScreen : MonoBehaviour
{
    public void Loader()
    {
        Invoke("NextLevel", 0.4f);
    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("FirstPlay", 1) == 1)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("FirstPlay", 0);
            SceneManager.LoadScene("game");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
