using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {
    private GameManager gameManager;
    private Rigidbody2D rb2d;
    private Goal goal;
    public Vector3 startPosition;
    private float currentSpeed = 2;
    private float temps;
    private float x;
    private float accelerationRate = 0.5f;

    void OnCollisionEnter2D(Collision2D col)
    {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider

        // Hit the left Racket?
        //MainCamera.GetComponent<RipplePostProcessor>().ShockWave();
        if (col.gameObject.name == "Player1")
        {
            
            // Calculate hit Factor
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1, y).normalized;

            // Set Velocity with dir * speed
            rb2d.velocity = dir * currentSpeed;
        }

        // Hit the right Racket?
        if (col.gameObject.name == "Player2")
        {
            // Calculate hit Factor
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1, y).normalized;

            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * currentSpeed;
        }
    }

    private void Start() {
        goal = GameObject.Find("Player1Goal").GetComponent<Goal>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {  
        temps += Time.deltaTime;
        if (temps > 1f)
        {
            BallAcceleration();
            temps = 0;
        }
    }
    private void Update()
    {

        rb2d.velocity = rb2d.velocity.normalized * currentSpeed;
    }
    public void ShotBall(){
        Debug.Log("Ball Launched");
        StartCoroutine(passiveMe(4));
        IEnumerator passiveMe(int secs)
        {
            yield return new WaitForSeconds(secs);
            gameManager.ballTrail.Play();
            if(gameManager.player1HasWin == false)
            {
                x = -1;
            }
            else if (gameManager.player1HasWin == true)
            {
                x = 1;
            }
            if (gameManager.gameHasStarted == false)
            {
                x = Random.Range(0, 2) == 0 ? 1 : -1;
                gameManager.gameHasStarted = true;
            }
            rb2d.velocity = (Vector2.one.normalized * currentSpeed) * new Vector2(x, 0);

            float step = currentSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, 0, 0), step);

        }
        
        
    }
    public void ResetAllPositions()
    {
        rb2d.velocity = Vector2.zero;
        currentSpeed = 2;
        gameManager.ball.transform.position = new Vector3(0, 0, 0);
        gameManager.player1Paddle.transform.position = new Vector3(-8.27f, 0, 0);
        gameManager.player2Paddle.transform.position = new Vector3(8.26f, 0, 0);
        gameManager.Launch();
    }
    

    
    
    
    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketHeight) {
    // ascii art:
    // ||  1 <- at the top of the racket
    // ||
    // ||  0 <- at the middle of the racket
    // ||
    // || -1 <- at the bottom of the racket
    return (ballPos.y - racketPos.y) / racketHeight;
}

    private void BallAcceleration(){
        if(currentSpeed < gameManager.maxSpeed)
        {
            currentSpeed += accelerationRate;
        }
        else
        {
            currentSpeed = gameManager.maxSpeed;
        }
        rb2d.velocity = rb2d.velocity.normalized * currentSpeed;
    }

    
}