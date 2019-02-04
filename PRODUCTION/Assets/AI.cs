using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    enum State { Idle = 0, Move, Climb }

    [SerializeField]
    enum Infiltration { Undetected, Detected, Trigger}

    private State currentState;

    private float currentDirection;
    private int currentFloor = 0;
    private int currentRoom = 0;

    private bool dirTrigger = false;
    private bool waitTrigger = true;

    #region Component
    private Rigidbody2D rgb2D;
    private SpriteRenderer sprRenderer;
    private Stage myStage;
    private Rooms myRooms;
    #endregion

    [SerializeField] private float speed;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Move;
        movement = Vector2.zero;
        currentDirection = -1;

        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo();
    }

    void MoveTo()
    {
        if(dirTrigger)
        {
            UpdateDirection();
        }

        if(currentState == State.Move)
            MoveDirection(currentDirection);

        if (currentState == State.Idle)
            rgb2D.velocity = Vector2.zero;

        Debug.Log("Current State: " + currentState.ToString());
    }

    void UpdateDirection()
    {
        if (currentRoom < myStage.GetNbRooms())
        {
            currentDirection = -1;
        }
        else
        {
            currentDirection = 1;
        }
    }

    void MoveDirection(float currentDirection)
    {
        // Sprite Gestion
        if (currentDirection < 0)
            this.sprRenderer.flipX = true;

        else
            this.sprRenderer.flipX = false;


        //Movement Script
        movement = new Vector2(speed * currentDirection, 0);
        this.rgb2D.velocity = movement * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Stage")
        {
            myStage = collision.GetComponent<Stage>();
            currentFloor = myStage.GetStage();
           
        }

        if(collision.tag == "Rooms")
        {
            myRooms = collision.GetComponent<Rooms>();
            currentRoom = myRooms.GetRoomNb();
            waitTrigger = false;
        }

        if(collision.tag == "TriggerRooms")
        {
            dirTrigger = true;
            
        }

        if(collision.tag == "TriggerWait" && waitTrigger == false)
        {
            waitTrigger = true;
            float waitChance = Random.Range(0, 10);
            if(waitChance <= 2)
                WaitPattern();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "TriggerRooms")
        {
            dirTrigger = false;
        }
    }

    private void WaitPattern()
    {
        
        if(currentState == State.Move)
        {
            float waiting = Random.Range(0.5f, 2.5f);
            StartCoroutine(WaitingTime(waiting));
  
            float waitingTime = Random.Range(3, 6);
            StartCoroutine(WaitingTime(waitingTime));
        }
    }

    IEnumerator WaitingTime(float time)
    {   
        yield return new WaitForSeconds(time);

        if (currentState != State.Move)
            currentState = State.Move;
        else
            currentState = State.Idle;
    }


}
