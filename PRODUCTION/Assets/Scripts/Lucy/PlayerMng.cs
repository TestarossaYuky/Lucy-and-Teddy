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
    private int currentStage;

    private bool canTP = false;
    private bool canHide = false;
    private bool isHide = false;

    private Vector3 currentPosition;
    #endregion

    #region Component
    private Rigidbody2D rgb2D;
    private SpriteRenderer sprRenderer;
    private Animator anim;

    private GameObject currentGate;
    private GameObject currentHide;
    #endregion

    #region State
    [SerializeField] 
    enum playerState { Idle, Move, Climb, Hide, Take, Crying}

    private playerState currentState;
    #endregion

    #region Climb
    private bool canClimb;

    private bool isDown;
    private bool isTop;

    private Transform finalLadder;

    private Transform ladderTransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentGate = null;
        currentHide = null;
        movement = Vector2.zero;
        SetState(playerState.Idle);

        currentPosition = GetComponent<Transform>().position;

        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != playerState.Climb && currentState != playerState.Hide)
            Move();

        Climb();
        Teleport();
        Hide();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case playerState.Idle:
                {
                    this.sprRenderer.enabled = true;
                    anim.SetBool("Move", false);
                    anim.SetBool("Idle", true);
                    
                    break;
                }

            case playerState.Move:
                {
                    this.sprRenderer.enabled = true;
                    anim.SetBool("Idle", false);
                    anim.SetBool("Move", true);
                    
                    break;
            
                }

            case playerState.Climb:
                {

                    break;

                }

            case playerState.Hide:
                {
                    this.sprRenderer.enabled = false;
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
                this.sprRenderer.flipX = true;
            
            else
                this.sprRenderer.flipX = false;

            movement = new Vector2(speed * inputX, 0);
            this.rgb2D.velocity = movement * Time.deltaTime;

        }
        else
            SetState(playerState.Idle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rooms")
        {
            currentRooms = collision.name;
        }

        if(collision.tag == "Stage")
        {
            currentStage = collision.GetComponent<Stage>().GetStage();
        }

        if(collision.tag == "Ladder")
        {
            canClimb = true;
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

        if (collision.name == "Down")
        {
            if(!isTop)
                isDown = true;  
        }

        if (collision.name == "Top")
        {
            if(!isDown)
                isTop = true;   
        }

        if(collision.tag == "Ennemi")
        {
            Destroy(this.gameObject);
        }
        
        if(collision.tag == "TP")
        {
            canTP = true;

            if (collision.name == "1" && canTP == true)
            {
                Teleport tp = collision.GetComponentInParent<Teleport>();
                currentGate = tp.enter2;
            }
            else if (collision.name == "2" && canTP == true)
            {
                Teleport tp = collision.GetComponentInParent<Teleport>();
                currentGate = tp.enter1;
            }
        }

        if(collision.tag == "Hide")
        {
            canHide = true;

            if(canHide)
            {
                currentHide = collision.gameObject;
            }
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

            else
                finalLadder = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ladder")
        {
            canClimb = false;
        }

        if (collision.tag == "Hide")
        {
            canHide = false;
        }
    }

    private void Hide()
    {
        if(canHide)
        {
            if(!isHide)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rgb2D.velocity = Vector2.zero;
                    isHide = true;
                    SetState(playerState.Hide);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rgb2D.velocity = Vector2.zero;
                    isHide = false;
                    SetState(playerState.Idle);
                }

            }
        }

    }

    private void Teleport()
    {
        if(canTP)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                this.transform.position = currentGate.transform.position;
                canTP = false;
            }
        }
    }

    private void Climb()
    {
        if(canClimb)
        {
            currentPosition.y = gameObject.transform.position.y;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rgb2D.velocity = Vector2.zero;
                SetState(playerState.Climb);
                this.gameObject.transform.position = new Vector2(ladderTransform.position.x, gameObject.transform.position.y);
                if(isDown)
                {
                    Up();
                }

                else if (isTop)
                {
                    Down();
                }
            }

            if(finalLadder != null)
            {
                if (isDown)
                {
                    if (currentPosition.y >= finalLadder.position.y + 0.2)
                    {
                        rgb2D.velocity = Vector2.zero;
                        canClimb = false;
                        SetState(playerState.Idle);
                    }
                }

                else if (isTop)
                {
                    if (currentPosition.y <= finalLadder.position.y + 0.2)
                    {
                        rgb2D.velocity = Vector2.zero;
                        canClimb = false;
                        SetState(playerState.Idle);
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
        if (currentPosition.y < finalLadder.position.y + 0.2)
        {
            Vector2 climb = new Vector2(0, speed);
            rgb2D.velocity = climb * Time.deltaTime;
        }      
    }

    private void Down()
    {
        if (currentPosition.y > finalLadder.position.y + 0.2)
        {
            Vector2 climb = new Vector2(0, -speed);
            rgb2D.velocity = climb * Time.deltaTime;
        }
    }
}
