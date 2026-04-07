using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Cutscene";
    public void PlayGame()
    {
        Debug.Log("Play");
        SceneManager.LoadScene(nextSceneName);
    }

    public void OpenCredits()
    {
        //Debug.Log("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
