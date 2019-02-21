using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    #region Variable
    [SerializeField] private float speed;
    private Vector2 movement;
    [SerializeField]
    private Rooms currentRooms;
    private int currentStage;

    private bool canTP = false;
    private bool canHide = false;
    private bool isHide = false;

    private bool isOn = false;

    private Vector3 currentPosition;

    public AudioClip door1;
    public AudioClip door2;

    #endregion

    #region Component
    private Rigidbody2D rgb2D;
    private SpriteRenderer sprRenderer;
    private Animator anim;

    private GameObject currentGate;
    private GameObject currentHide;

    private Teleport currentTp;

    private SpriteRenderer sprIcone;
    #endregion

    #region State
    [SerializeField] 
    enum playerState { Idle, Move, Climb, Hide, Take, Crying, Door}

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
        sprIcone = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != playerState.Climb && currentState != playerState.Hide && currentState != playerState.Door)
            Move();

        Climb();
        Teleport();
        Hide();

        if (currentRooms != null)
            isOn = currentRooms.GetIsOn();
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
            case playerState.Door:
                {
                    this.sprRenderer.enabled = false;
                    this.sprIcone.enabled = false;
                    break;
                }
        }
            
    }

    void SetState(playerState newState)
    {
        currentState = newState;
    }

    public int GetCurrentStage()
    {
        return currentStage;
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
            currentRooms = collision.GetComponent<Rooms>();
        }

        if(collision.tag == "Stage")
        {
            currentStage = collision.GetComponent<Stage>().GetStage();
        }

        if(collision.tag == "Ladder")
        {
            canClimb = true;
            sprIcone.enabled = true;
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

        if(collision.tag == "Ennemi" && currentState != playerState.Hide)
        {
            this.gameObject.SetActive(false);
            print("You lose");
        }

        if (collision.tag == "Item" && collision.GetComponent<SpriteRenderer>().enabled == true)
        {
            collision.gameObject.GetComponent<AudioSource>().Play();
            collision.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(WaitItem(collision.gameObject.GetComponent<AudioSource>()));
            print("You Win");
        }
    }

    IEnumerator WaitItem(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source.gameObject);
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

        if (isOn == true)
        {
            if (collision.tag == "Hide")
            {

                canHide = true;

                if (canHide)
                {
                    sprIcone.enabled = true;
                    currentHide = collision.gameObject;
                }
            }

            if (collision.tag == "TP")
            {
                canTP = true;
                if(canTP == true && currentState != playerState.Door)
                    sprIcone.enabled = true;

                if (collision.name == "1" && canTP == true)
                {
                    currentTp = collision.GetComponentInParent<Teleport>();
                    currentGate = currentTp.enter2;
                }
                else if (collision.name == "2" && canTP == true)
                {
                    currentTp = collision.GetComponentInParent<Teleport>();
                    currentGate = currentTp.enter1;
                }
            }
        }
        else
        {
            canTP = false;
            if (canTP == false && currentState != playerState.Hide && canClimb == false)
                sprIcone.enabled = false;
        }
            
    }
        private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ladder")
        {
            canClimb = false;
            sprIcone.enabled = false;
        }

        if (collision.tag == "Hide")
        {
            canHide = false;
            sprIcone.enabled = false;
        }

        if (collision.tag == "TP")
        {
            canTP = false;
            sprIcone.enabled = false;
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
                //this.transform.position = currentGate.transform.position;
                SetState(playerState.Door);
                if (currentTp.GetComponent<AudioSource>() != null)
                    StartCoroutine(WaitDoor(currentTp.GetComponent<AudioSource>()));
                canTP = false;
            }
        }
    }

    IEnumerator WaitDoor(AudioSource source)
    {
        int i = Random.Range(0, 2);
        if(i == 0)
        {
            source.clip = door1;
            source.PlayOneShot(door1, 0.5f);
        }
        else
        {
            source.clip = door2;
            source.PlayOneShot(door1, 0.5f);
        }
 
        yield return new WaitForSeconds(source.clip.length - 0.5f);
        this.transform.position = currentGate.transform.position;
        SetState(playerState.Idle);
    }

    private void Climb()
    {
        if(canClimb)
        {
            currentPosition.y = gameObject.transform.position.y;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                sprIcone.enabled = false;
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
