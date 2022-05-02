using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb2d;

    // private Rigidbody2D rb;
    public KeyCode moveUp = KeyCode.UpArrow;
    public KeyCode moveDown = KeyCode.DownArrow;
    private float speed = 4.5f;
    private float boundY = 3;
    // Start is called before the first frame update
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(moveUp))
        {
            float step = gameManager.aiSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, boundY, 0), step);
        }
        else if (Input.GetKey(moveDown))
        {
            float step = gameManager.aiSpeed * Time.deltaTime;
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
