﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

enum Direction
{
    TopLeft,
    BottomLeft,
    TopRight,
    BottomRight,
}

public class CircleController : MonoBehaviour
{   
    public float initialMoveSpeed = 10f;
    public float moveSpeed;
    public float rotationSpeed = 180f;
    public float touchBuffer = 0.1f;
    private Rigidbody2D circle;
    private Direction direction;
    float timeToTouch = 1.0f;
    float timeLeft;
    private int score;
    public Text scoreText;
    private float bufferTimer;
    private bool isTouching = false;

    void Start() {
        circle = GetComponent<Rigidbody2D>();
        direction = Direction.BottomRight;
        timeLeft = timeToTouch;
        score = 0;
        scoreText.text = "0";
        moveSpeed = initialMoveSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Vector2.zero;

        switch (direction)
        {
            case Direction.TopLeft:
                movement.x -= (transform.right).x;
                movement.y += (transform.up).y;
                break;
            case Direction.TopRight:
                movement.x += (transform.right).x;
                movement.y += (transform.up).y;
                break;
            case Direction.BottomLeft:
                movement.x -= (transform.right).x;
                movement.y -= (transform.up).y;
                break;
            case Direction.BottomRight:
                movement.x += (transform.right).x;
                movement.y -= (transform.up).y;
                break;
            default:
                Debug.Log("Direction Unknown");
                break;
        }

        movement.Normalize();

        movement = movement * moveSpeed;
        circle.MovePosition((Vector2)(transform.position) + movement * Time.deltaTime);

        if (bufferTimer >= 0 && !isTouching) {
            bufferTimer -= Time.deltaTime;
        }

        if (bufferTimer <= 0 && !isTouching) {
            UnDock();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isTouching = true;
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.30196078f, 0.30196078f);
       
    }

    private void OnTriggerStay2D(Collider2D other)
	{
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = timeToTouch;
            System.Array values = System.Enum.GetValues(typeof(Direction));
            direction = (Direction)values.GetValue(Random.Range(0, values.Length));
            score++;
            moveSpeed = score + initialMoveSpeed;
            scoreText.text = score.ToString();
        }

        bufferTimer = touchBuffer;
	}

	void OnTriggerExit2D(Collider2D other)
	{
        isTouching = false;
        bufferTimer = touchBuffer;
	}

    void UnDock() {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        timeLeft = timeToTouch;
    }
}