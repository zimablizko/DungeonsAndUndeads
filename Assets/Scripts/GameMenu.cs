using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject soundButtonText;

    private void Start()
    {
        UpdateSoundText();
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }
    
    public void OnClickInventory()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            inventory.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            inventory.SetActive(false);
        }
    }

    public void OnSoundClick()
    {
        GameManager.Instance.isSoundEnabled = !GameManager.Instance.isSoundEnabled;
        UpdateSoundText();
        PlayerPrefs.SetInt("Sound_Enabled", Convert.ToInt16(GameManager.Instance.isSoundEnabled));
    }

    public void OnClickResume()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void OnClickGoToMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    private void UpdateSoundText()
    {
        if (GameManager.Instance.isSoundEnabled)
            soundButtonText.GetComponent<Text>().text = "Sound: On";
        else
            soundButtonText.GetComponent<Text>().text = "Sound: Off";
    }
}