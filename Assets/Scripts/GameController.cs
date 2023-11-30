using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameController : NetworkBehaviour
{   
    [Tooltip("In minutes")]
    public bool useTimer = false;
    public float maxGameTime = 1f;
    public bool gameStarted = false;
    public Loot objective;


    UIController mainUI;
    float currentGameTime;
    private float gameTimeScale = 60f;
    private GameObject client;
    private GameObject host;

    // [SyncVar(hook = nameof(syncGameOverState))]
    public bool isGameOver = false; 
    private bool endGameActive = false;
    private int hostScore;
    private int clientScore;


    //TODO: this was all implemented really quickly and sloppily due to time constraints. Redo it.

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
        if(isServer && isGameOver && !endGameActive) endGame(); 
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

    public void callEndGame(){
        if(isServer) endGame();
        else CmdEndGame();
    }

    public Loot gameObjective(){
        return objective;
    }

    private void endGame(){
        print("ENDGAME");
        endGameActive = true;
        var hostInventory = getHostInventory();
        hostScore = calculateScore(hostInventory);
        print(host);
        print(hostInventory.Count);
        print("HostSCore " + hostScore);
        mainUI.enableEndGame(true);
        mainUI.setHostScore(hostScore);
        RpcSetEndGame(hostScore);
    }

    void setWinner(int hostScore, int clientScore){
        string winner;
        if(hostScore == clientScore){
            winner = "TIE";
        }
        else if(hostScore == Mathf.Max(hostScore, clientScore)){
            winner = "HOST";
        }
        else{
            winner = "CLIENT!";
        }

        mainUI.setWinner(winner);
    }

    List<Loot> getHostInventory(){
        return host.GetComponent<PlayerInventory>().inventory;
    }

    List<Loot> getClientInventory(){
        return client.GetComponent<PlayerInventory>().inventory;
    }

    public void registerPlayer(GameObject player, bool isHost){
        print("Register " + isHost); 
        if(isHost) host = player;
        else client = player;
    }
    
    int calculateScore(List<Loot> lootList){
        var score = 0;
        foreach(Loot loot in lootList){
            score += loot.value;
        }
        return score;
    }


    [Command(requiresAuthority = false)]
    void CmdSetClientScore(int score){
        mainUI.setClientScore(score);
        setWinner(hostScore, score);
    }

    [ClientRpc]
    void RpcSetEndGame(int hostScore){
        if(!isServer) {
            var clientInventory = getClientInventory();
            var clientScore = calculateScore(clientInventory);
            CmdSetClientScore(clientScore);
            mainUI.enableEndGame(true);
            mainUI.setHostScore(hostScore);
            mainUI.setClientScore(clientScore);
            setWinner(hostScore, clientScore);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdEndGame(){
        print("COMMAND");
        endGame();
    }

}
