using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject soundButtonText;

    private void Start()
    {
        UpdateSoundText();
        AudioManager.Instance.PlayTheme("MainTheme");
    }

    public void OnClickPause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            //EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Resume_btn"));
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
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
        pauseMenu.SetActive(false);
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

    public void OnDefeatMenu()
    {
        defeatMenu.SetActive(true);
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("MainMenu_btn"));
    }    
    
    public void OnVictoryMenu()
    {
        victoryMenu.SetActive(true);
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("MainMenu_btn"));
    }
}