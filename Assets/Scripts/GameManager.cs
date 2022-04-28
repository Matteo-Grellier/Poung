using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Particles")]
    public ParticleSystem ballTrail;
    public BallControl ballController;
    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    public GameObject player1Paddle;
    public GameObject player1Goal;
    public int paddleSpeed;
    [Header("Player 2")]
    public GameObject player2Paddle;
    public GameObject player2Goal;

    [Header("AI")]
    public float aiSpeed;

 
    [Header("Score UI")]
    public GameObject Player1Text;
    public GameObject Player2Text;

    private int Player1Score;
    private int Player2Score;

    public void Player1Scored(){
        Debug.Log("Player 1 Scored");
        Player1Score++;

        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
    }
    public void Player2Scored(){
        Player2Score++;
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
    }
    public void EndGame(){
        Debug.Log("Game Over");
        Player1Score = 0;
        Player2Score = 0;
        Player1Text.GetComponent<TextMeshProUGUI>().text = Player1Score.ToString();
        Player2Text.GetComponent<TextMeshProUGUI>().text = Player2Score.ToString();
    }
    private void Update() {
        if(Player1Score == 5){
            EndGame();
        }
        if(Player2Score == 5){
            EndGame();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Reset();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            Reset();
        }
    }

    public void Launch(){
        ballController.ShotBall();
    }

    public void Reset(){
        ballTrail.Stop();
        ballController.ResetAllPositions();
    }

    private void Start() {
        ballController = ball.GetComponent<BallControl>();
        ballTrail.Stop();

        // stop particles from playing
        Launch();

        ballController = ball.GetComponent<BallControl>();
    }
}
