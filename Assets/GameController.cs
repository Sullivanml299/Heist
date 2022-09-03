using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{   
    [Tooltip("In minutes")]
    public bool useTimer = false;
    public float maxGameTime = 1f;
    public bool gameStarted = false;


    UIController mainUI;
    float currentGameTime;
    private float gameTimeScale = 60f;

    // Start is called before the first frame update
    void Start()
    {
        mainUI = GameObject.Find("MainUI").GetComponent<UIController>();
        if(useTimer){
            mainUI.enableTimer(useTimer);
            currentGameTime = maxGameTime * gameTimeScale;
            updateGameTime();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(useTimer && gameStarted) updateGameTime();
    }

    void updateGameTime(){
        if(currentGameTime>0) {
            print("UPDATE TIME");
            if(gameStarted) currentGameTime -= Time.deltaTime;
            int minutes = (int) currentGameTime/60;
            int seconds = (int) currentGameTime%60;
            mainUI.updateTimeRemaining(minutes, seconds);
        }
    }

    void startGame(){
        gameStarted = true;
    }
}
