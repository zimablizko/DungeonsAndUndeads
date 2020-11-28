using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    public Player player;
    public GameObject canvas;

    public GameWorld gameWorld;
    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>();
        canvas = GameObject.Find("Canvas");
        
        gameWorld = new GameWorld();
    }

    // Start is called before the first frame update
    public void ChangeScene()
    {
        canvas.GetComponent<FadeScreen>().FadeIn();
        player.StopAllCoroutines();
        player.gameObject.SetActive(false);
        StartCoroutine(ChangeSceneFade());
    }

    private IEnumerator ChangeSceneFade()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.UnloadSceneAsync(gameWorld.CurrentRoom.Name);
        StartCoroutine(AsyncLoad(gameWorld.GetNextRoom().Name));
        canvas.GetComponent<FadeScreen>().FadeOut();
        
        gameWorld.SetNextRoom();
    }
    
    public void LoadFirstScene()
    {
        StartCoroutine(AsyncLoad(gameWorld.GetFirstRoom().Name));
    }

    IEnumerator AsyncLoad(string sceneName)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadingOperation.isDone)
        {
            Debug.Log(loadingOperation.progress);
            yield return null;
        }
        player.gameObject.SetActive(true);
        canvas.GetComponent<FadeScreen>().FadeOut();
        player.transform.position = GameObject.FindWithTag("StartPosition").transform.position;
    }
}
