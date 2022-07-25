using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        IN_GAME,
        PAUSED,
        IN_MENU
    } 

    private GameState gameState = GameState.IN_GAME;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;            
    }

    // Update is called once per frame
    void Update()
    {       
        CheckInput();
    }
        
    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameState != GameState.PAUSED)
            {
                Time.timeScale = 0;
                gameState = GameState.PAUSED;
            }
            else
            {
                Time.timeScale = 1;
                gameState = GameState.IN_GAME;
            }
        }
    }
}
