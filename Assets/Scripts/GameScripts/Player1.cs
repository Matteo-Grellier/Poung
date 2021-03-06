using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    private Rigidbody2D rb2d;

    // private Rigidbody2D rb;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    private float boundY = 4.5f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update () {
        if (Input.GetKey(moveUp))
        {
            float step = gameManager.paddleSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, boundY, 0), step);
        }
        else if (Input.GetKey(moveDown))
        {
            float step = gameManager.paddleSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, boundY, 0), -step);
        }
        else
        {
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


