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
    public float boundY;
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
            rb2d.velocity = new Vector2(0, speed);
        }
        else if (Input.GetKey(moveDown))
        {
            rb2d.velocity = new Vector2(0, -speed);
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
