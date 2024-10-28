using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EndGameManager : MonoBehaviour
{
    public int moveCount;
    public int timeCount;
    private float time;

    public GameObject YouWinPanel;
    public GameObject TryAgainPanel;
   
    public Text moveCounter;
    public Text timeCounter;

    private Board board;



    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();

        time = 1;
        moveCounter.text = "" + moveCount;
    }

    public void DecreaseCounterMove()
    {

       
            moveCount--;
            moveCounter.text = "" + moveCount;
        if(moveCount <= 0)
        {
            TryAgainPanel.SetActive(true);
            board.currentState = GameState.lose;
            Debug.Log("Lose");
            moveCount = 0;
            moveCounter.text = "" + moveCount;
            FadePanelController fade = FindObjectOfType<FadePanelController>();
            fade.GameOver();
        }
            
        
    }

    private void DecreaseCounterTime()
    {
        if (board.currentState != GameState.pause)
        {
            if (timeCount >= 1)
            {
                timeCount--;
                timeCounter.text = "" + timeCount;
            }
        }
    }

    public void WinGame()
    {
        YouWinPanel.SetActive(true);
        board.currentState = GameState.win;
        timeCount = 0;
        moveCount = 0;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
       
    }

    

    // Update is called once per frame
    void Update()
    {
        if (timeCount > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                DecreaseCounterTime();
                time = 1;
            }
        }
        else
        {
            TryAgainPanel.SetActive(true);
            board.currentState = GameState.lose;
            FadePanelController fade = FindObjectOfType<FadePanelController>();
            fade.GameOver();
            Debug.Log("Lose");
        }

    }
}
