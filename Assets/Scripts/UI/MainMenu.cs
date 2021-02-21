using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
  /*[SerializeField] private InputField nameField;

  

  public void OnEndEditName()
  {
    PlayerPrefs.SetString("Player_Name",nameField.text);
  }*/
  public int maxDifficulty;
  public int difficulty;
  public GameObject difficultyText;
  private void Start()
  {
    OnBegin();
    AudioManager.Instance.PlayTheme("MenuTheme");
    if (PlayerPrefs.HasKey("Max_Difficulty"))
      maxDifficulty = PlayerPrefs.GetInt("Max_Difficulty");
    else
    {
      maxDifficulty = 1;
      PlayerPrefs.SetInt("Max_Difficulty", 1);
      PlayerPrefs.Save();
    }
    difficulty = maxDifficulty;
    difficultyText.GetComponent<Text>().text = maxDifficulty.ToString();
  }
  
  public void OnBegin()
  {
    EventSystem.current.SetSelectedGameObject(null);
    EventSystem.current.SetSelectedGameObject(GameObject.Find("Start_btn"));
  }  
  
  public void OnClickStart()
  {
    EventSystem.current.SetSelectedGameObject(null);
    EventSystem.current.SetSelectedGameObject(GameObject.Find("Play_btn"));
  }    
  
  public void OnClickControlPanel()
  {
    EventSystem.current.SetSelectedGameObject(null);
    EventSystem.current.SetSelectedGameObject(GameObject.Find("Back_btn"));
  }  
  
  public void OnClickPlay()
  {
    PlayerPrefs.SetInt("Difficulty", difficulty);
    PlayerPrefs.Save();
    SceneManager.LoadScene("gameplay");
    Time.timeScale = 1;
  }

  public void OnClickExit()
  {
    Application.Quit();
  }
  
  #region Difficulty

  public void UpDifficulty()
  {
    difficulty = Math.Min(maxDifficulty, difficulty + 1);
    difficultyText.GetComponent<Text>().text = difficulty.ToString();
  }

  public void DownDifficulty()
  {
    difficulty = Math.Max(1, difficulty - 1);
    difficultyText.GetComponent<Text>().text = difficulty.ToString();
  }

  #endregion
}
