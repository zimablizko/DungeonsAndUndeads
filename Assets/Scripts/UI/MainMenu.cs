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
  private void Start()
  {
    OnBegin();
    AudioManager.Instance.PlayTheme("MenuTheme");
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
  
  public void OnClickPlay()
  {
    SceneManager.LoadScene("gameplay");
    Time.timeScale = 1;
  }

  public void OnClickExit()
  {
    Application.Quit();
  }
}
