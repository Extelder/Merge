using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLodeLastLevel : MonoBehaviour
{
    public static SaveAndLodeLastLevel Singleton { get; private set; }

    public bool _start = true;
    
    private void Awake()
    {
        enabled = false;
        Singleton = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("LastSceneIndex") && (PlayerPrefs.GetInt("LastSceneIndex") != SceneManager.GetActiveScene().buildIndex) && _start == true)
        {
            SceneManager.LoadScene(GetLastLevelIndex());
        }
    }

    public void InvokeSaveWithWait()
    {
        SaveLastLevelIndex();
    }

    public void OnSceneSwitched()
    {
        SaveLastLevelIndex();
    }

    private void SaveLastLevelIndex()
    {
        PlayerPrefs.SetInt("LastSceneIndex", SceneManager.GetActiveScene().buildIndex);
    }

    private int GetLastLevelIndex()
    {
        return PlayerPrefs.GetInt("LastSceneIndex");
    }
}