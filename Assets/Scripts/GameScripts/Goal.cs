using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager gameManager;
    public bool isPlayer1Goal;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (!isPlayer1Goal)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Player1Scored();
                gameManager.player1HasWin = true;
                gameManager.Reset();
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().Player2Scored();
                gameManager.player1HasWin = false;
                gameManager.Reset();
            }
            
        }
    }
    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    

}
