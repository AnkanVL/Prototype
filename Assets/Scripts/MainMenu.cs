using UnityEditor.SearchService;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        
        SceneManager.LoadScene("Setting");
        
    }
    public void AudioSettings()
    {
        SceneManager.LoadScene("AudioSetting");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("User Quit Game");
    }

}