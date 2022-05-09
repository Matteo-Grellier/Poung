using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb2d;
    private BallControl ballControl;
    private float boundY = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        ballControl = GameObject.Find("Ball").GetComponent<BallControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Mathf.Abs(gameManager.ball.transform.position.y-transform.position.y) > 1 && Mathf.Abs(gameManager.ball.transform.position.y-transform.position.y) < 1.5);
        if(ballControl.leftPaddleHasTouch)
        {
            moveAI();
        
        }else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }
    private void moveAI(){
        
            if(gameManager.ball.transform.position.y > transform.position.y+0.5){
                float step = gameManager.aiSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, boundY, 0), step);
            }
            else if(gameManager.ball.transform.position.y < transform.position.y){
                float step = gameManager.aiSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, boundY, 0), -step);
            }
            else{
                rb2d.velocity = new Vector2(0, 0);
            }
            if (transform.position.y > boundY)
            {
                transform.position = new Vector3(transform.position.x, boundY, transform.position.z);
            }
            else if (transform.position.y < -boundY)
            {
                transform.position = new Vector3(transform.position.x, -boundY, transform.position.z);
            }
    }
}
