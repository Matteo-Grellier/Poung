using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameObject MainCamera;
    private GameManager gameManager;
    private bool isGoal = false;
    public bool hasColide = false;

    public bool isPlayer1Goal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (!isPlayer1Goal)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Player1Scored();
                hasColide = true;
                gameManager.player1HasWin = true;
                gameManager.Reset();
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Player2Scored();
                gameManager.player1HasWin = false;
                hasColide = true;

                gameManager.Reset();
            }
            
        }
    }
    private void Start() {
        MainCamera = GameObject.Find("MainCamera");
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    

}
