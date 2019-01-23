using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    #region Variable
    [SerializeField] private float speed;
    private Vector2 movement;
    [SerializeField]
    private string currentRooms;
    #endregion

    #region Component
    private Rigidbody2D rgb2D;
    private SpriteRenderer sprRenderer;
    #endregion

    #region State
    [SerializeField] 
    enum playerState { Idle, Move, Climb, Hide, Take, Crying}

    private playerState currentState;
    #endregion

    #region Class
    //private RoomsMng currentRooms;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movement = Vector2.zero;
        SetState(playerState.Idle);


        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();

        //currentRooms = RoomsMng.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);

        if (currentState != playerState.Climb) ;
            Move();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case playerState.Idle:
                {

                    break;
                }

            case playerState.Move:
                {
                    // anim
                    break;
            
                }

            case playerState.Climb:
                {
                    break;

                }

            case playerState.Hide:
                {
                    break;
                }

        }
            
    }

    void SetState(playerState newState)
    {
        currentState = newState;
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (inputX != 0)
        {
            SetState(playerState.Move);

            if (inputX < 0)
                this.sprRenderer.flipY = true;
            
            else
                this.sprRenderer.flipX = false;

            movement = new Vector2(speed * inputX, 0);
            this.rgb2D.velocity = movement * Time.deltaTime;

        }
        else
            SetState(playerState.Idle);
    }
        
    private void CheckRooms()
    {
        // tu check collision etcurrentroom =string 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rooms")
        {
            currentRooms = collision.name;
        }
    }
}
