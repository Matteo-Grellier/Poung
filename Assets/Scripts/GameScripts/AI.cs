using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb2d;

    private float boundY = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Mathf.Abs(gameManager.ball.transform.position.y-transform.position.y) > 1 && Mathf.Abs(gameManager.ball.transform.position.y-transform.position.y) < 1.5);
        if(gameManager.ball.transform.position.x > 5)
        {
            moveAI();
        
        }else
        {
            rb2d.velocity = new Vector2(0, 0);
        }
    }
    private void moveAI(){
        
            if(gameManager.ball.transform.position.y > transform.position.y+0.5){
                rb2d.velocity = new Vector2(0, gameManager.aiSpeed);
                Debug.Log("1");
            }
            else if(gameManager.ball.transform.position.y < transform.position.y){
                rb2d.velocity = new Vector2(0, -gameManager.aiSpeed);
                Debug.Log("2");
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
