using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class GameManager : MonoBehaviour {

    static public GameManager S;
    public bool gameOver { get; private set; }
    public bool isPaused = false;
    public bool playTutorial = true;
    public bool isContinue = false;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        SetGameOver(false);
        UIManager.S.ToggleGameOverPanel(false);
        UIManager.S.ToggleHitPausePanel(false);
        Pause(true);
        playTutorial = DataManager.S.GetTutorial() == 1;
        isContinue = false;
    }

    void Start()
    {
        StartCoroutine(CountDownToPlay());
    }

    public void SetGameOver(bool isOver)
    {
        gameOver = isOver;
        UIManager.S.ToggleGameOverPanel(gameOver);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

    public void Pause(bool pause)
    {
        isPaused = pause;
    }

    public void Continue()
    {
        //check to see if user has 200 coins
        isContinue = true;
        if(DataManager.S.GetCoinsAmt() >= 200)
        {
            DataManager.S.SetCoins(DataManager.S.GetCoinsAmt() - 200);
            StartCoroutine(Camera.main.GetComponent<CameraFollow>().ToggleBrokenScreen(false));
            StartCoroutine(CountDownToPlay());
        }
        else
        {
            UIManager.S.ToggleResultPanel(false);
            UIManager.S.ToggleContinuePanel(true);
        }
        //if (true){
        //pay 200 coins
        // countdown to continue game
        //continue game
        //}
        //else{
        // show continuePanel
        //show "you do not have enough coins" message?

    }

    IEnumerator CountDownToPlay()
    {
        StartCoroutine(UIManager.S.Countdown());
        yield return null;
        
    }
    
}
