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

    private Vector3 currentPosition;

    #region Component
    private Rigidbody2D rgb2D;
    private SpriteRenderer sprRenderer;
    private Stage myStage;
    private Rooms myRooms;
    private Animator anim;
    #endregion

    #region Climb
    private bool canClimb;

    private bool isDown;
    private bool isTop;

    private Transform finalLadder;

    private Transform ladderTransform;
    #endregion

    [SerializeField] private float speed;
    private Vector2 movement;

    private float ladderChance;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Move;
        movement = Vector2.zero;
        currentDirection = -1;

        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo();
        Climb();
        
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Idle:
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("Idle", true);

                    break;
                }

            case State.Move:
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    break;

                }

            case State.Climb:
                {

                    break;

                }


        }

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

        if (collision.tag == "Ladder")
        {
            float value = Random.Range(0f, 1f);
            ladderChance = collision.GetComponent<Ladder>().GetAiChance();
            if (value <= ladderChance)
            {
                canClimb = true;
                collision.GetComponent<Ladder>().SetClimb(true);
                ladderTransform = collision.transform;
                if (isDown)
                {
                    finalLadder = collision.gameObject.transform.GetChild(0).transform;
                }

                else if (isTop)
                {
                    finalLadder = collision.gameObject.transform.GetChild(1).transform;
                }
            }
        }

        if (collision.name == "Down")
        {
            if (!isTop)
                isDown = true;
        }

        if (collision.name == "Top")
        {
            if (!isDown)
                isTop = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            if (isDown)
            {
                finalLadder = collision.gameObject.transform.GetChild(0).transform;
            }

            else if (isTop)
            {
                finalLadder = collision.gameObject.transform.GetChild(1).transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "TriggerRooms")
        {
            dirTrigger = false;
        }

        if (collision.tag == "Ladder")
        {
            canClimb = false;
            finalLadder = null;
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

    private void Climb()
    {
        if (canClimb)
        {
            currentPosition.y = gameObject.transform.position.y;
     
                rgb2D.velocity = Vector2.zero;
                currentState = State.Climb;
                this.gameObject.transform.position = new Vector2(ladderTransform.position.x, gameObject.transform.position.y);
                if (finalLadder != null)
                {
                    if (isDown)
                    {
                        Up();
                    }

                    else if (isTop)
                    {
                        Down();
                    }

                    if (isDown)
                    {
                        if (currentPosition.y >= finalLadder.position.y + 0.3)
                        {
                            rgb2D.velocity = Vector2.zero;
                            canClimb = false;
                            currentState = State.Move;
                            int randomDir = Random.Range(0, 1);
                            if(randomDir == 0)
                                currentDirection *= -1;
                            // check room
                        }
                    }

                    else if (isTop)
                    {
                        if (currentPosition.y <= finalLadder.position.y + 0.3)
                        {
                            rgb2D.velocity = Vector2.zero;
                            canClimb = false;
                            currentState = State.Move;
                            int randomDir = Random.Range(0, 1);
                            if (randomDir == 0)
                                currentDirection *= -1;
                            //check room
                        }
                    }
                
            }         
        }

        else
        {
            isDown = false;
            isTop = false;
        }        
    }

    private void Up()
    {
        if (currentPosition.y < finalLadder.position.y + 0.3)
        {
            Vector2 climb = new Vector2(0, speed);
            rgb2D.velocity = climb * Time.deltaTime;
        }
    }

    private void Down()
    {
        if (currentPosition.y > finalLadder.position.y + 0.3)
        {
            Vector2 climb = new Vector2(0, -speed);
            rgb2D.velocity = climb * Time.deltaTime;
        }
    }


}
