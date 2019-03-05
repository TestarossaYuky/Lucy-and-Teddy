﻿using System.Collections;
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
    public GameMng myGameMng;
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
    enum playerState { Idle, Move, Climb, Hide, Take, Crying, Door, Dialogue, Interact}

    [SerializeField]
    private playerState currentState;
    #endregion

    #region Climb
    private bool canClimb;

    private bool isDown;
    private bool isTop;

    private Transform finalLadder;

    private Transform ladderTransform;
    #endregion

    #region Tutorial
    private bool canInteract = false;

    private bool isFirstMove = false;
    private bool isFirstLight = false;
    private bool isFirstJack = false;
    private bool LightClear = false;
    private bool dialogueEnd = false;
    private bool climbTutorial = false;

    private bool haveKey = false;
    private bool nextToDoor = false;
    private GameObject currentDoor;
    public GameObject KeyKitchen;
    public GameObject JackTrigger;

    private bool fridgeOpen = false;
    private bool fridgeCollider = false;

    private bool step1 = false;
    private bool step2 = false;
    private bool step3 = false;

    private AudioSource TeddySource;

    public AudioClip MoveDialogue;
    public AudioClip HugDialogue;
    public AudioClip SetLight;
    public AudioClip JackDialogue;


    public AudioClip DoorLock;
    public AudioClip DoorUnLock;
    public AudioClip FridgeOpen;
    public AudioClip FridgeClose;

    public InteractableObject phono;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentDoor = null;
        currentGate = null;
        currentHide = null;
        movement = Vector2.zero;
        SetState(playerState.Dialogue);

        currentPosition = GetComponent<Transform>().position;

        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sprIcone = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        TeddySource = this.transform.GetChild(1).GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OpenDoor(haveKey);
        Fridge();
        if(myGameMng.currentState == GameMng.GameState.Tutorial)
        {            
            
            if(currentState == playerState.Dialogue && step1 == false)
            {
                StartCoroutine(PlayDialogue(MoveDialogue));
            }
            if(step1 == true && currentState != playerState.Dialogue)
            {
                Move();
            }
            if (climbTutorial == true && currentState != playerState.Dialogue)
            {
                Climb();
                Teleport();
                Hide();
            }

            if(step2 == true)
            {
                isFirstLight = true;
              
            }

            if(step3 == true)
            {
                isFirstJack = true;
            }

            if(phono.GetListen() == true)
            {
                myGameMng.SetState(GameMng.GameState.First);
            }
        }

        else
        {
            
            if (currentState != playerState.Climb && currentState != playerState.Hide && currentState != playerState.Door)
                Move();

            Climb();
            Teleport();
            Hide();

            
        }
        if (currentRooms != null)
            isOn = currentRooms.GetIsOn();
    }

    IEnumerator PlayDialogue(AudioClip clip)
    {
        SetState(playerState.Dialogue);

        if (clip == MoveDialogue)
        {
            step1 = true;
        }

        else if (clip == SetLight)
        {
            step2 = true;
        }

        else if(clip == JackDialogue)
        {
            step3 = true;
        }

        else if(clip == HugDialogue)
        {
            climbTutorial = true;
        }

        TeddySource.PlayOneShot(clip, 1f);

        yield return new WaitForSeconds(clip.length);

        

        SetState(playerState.Idle);
    }

    public bool GetJack()
    {
        return isFirstJack;
    }

    public void SetJack(bool x)
    {
        isFirstJack = x;
    }

    public bool GetClear()
    {
        return LightClear;
    }

    public void SetClear(bool x)
    {
        LightClear = x;
    }

    public bool GetFirstLight()
    {
        return isFirstLight;
    }

    public void SetFirstLight(bool x)
    {
        isFirstLight = x;
    }

    public bool GetStep2()
    {
        return step2;
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
            case playerState.Dialogue:
                {
                    this.rgb2D.velocity = Vector2.zero;
                    anim.SetBool("Idle", true);
                    anim.SetBool("Move", false);
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


    private void Fridge()
    {
        if(fridgeCollider == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (fridgeOpen == true && haveKey == true)
                {
                    print("close");
                    if(KeyKitchen != null)
                        KeyKitchen.GetComponent<SpriteRenderer>().enabled = false;
                    //animation fermer
                    fridgeOpen = false;
                }
                else if (fridgeOpen == false)
                {
                    print("open");
                    if (KeyKitchen != null)
                        KeyKitchen.GetComponent<SpriteRenderer>().enabled = true;
                    //animation ouvert
                    fridgeOpen = true;
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tutorial

        if(collision.name == "Fridge")
        {
            canInteract = true;
            sprIcone.enabled = true;
            fridgeCollider = true;
                
            
        }

        if(collision.name == "JackTrigger")
        {
            StartCoroutine(PlayDialogue(JackDialogue));
            Destroy(collision.gameObject);
        }

        if(collision.name == "FirstClimbTrigger")
        {         
            StartCoroutine(PlayDialogue(HugDialogue));
            Destroy(collision.gameObject); 
        }

        if(collision.name == "FirstLightTrigger")
        {
            StartCoroutine(PlayDialogue(SetLight));
            Destroy(collision.gameObject);
        }
        //

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Lock")
        {
            sprIcone.enabled = true;
            nextToDoor = true;
            canInteract = true;
            currentDoor = collision.gameObject;
        }
    }

    private void OpenDoor(bool doorKey)
    {
        if(nextToDoor == true  && currentDoor != null)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(doorKey == true)
                {
                    JackTrigger.SetActive(true);
                    Destroy(currentDoor);
                    nextToDoor = false;
                    doorKey = false;
                    // play is unlock
                }
                else
                    StartCoroutine(PlayDialogue(MoveDialogue));
                //lockSound
            }
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
      
        if(fridgeOpen == true)
        {
            if(collision.name == "KeyFirst")
            {
                canInteract = true;
                sprIcone.enabled = true;
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Destroy(collision.gameObject);
                    haveKey = true;
                }
                
            }
        }

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
            if (canTP == false && currentState != playerState.Hide && canClimb == false  && canInteract == false)
                sprIcone.enabled = false;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Fridge")
        {
            sprIcone.enabled = false;
            canInteract = false;
            fridgeCollider = false;
        }

        if (collision.tag == "Lock")
        {
            sprIcone.enabled = false;
            nextToDoor = false;
            currentDoor = null;
            canInteract = false;
        }

        if (collision.tag == "Ladder")
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

        if (canClimb)
        {

            currentPosition.y = gameObject.transform.position.y;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                sprIcone.enabled = false;
                rgb2D.velocity = Vector2.zero;
                SetState(playerState.Climb);
                this.gameObject.transform.position = new Vector2(ladderTransform.position.x, gameObject.transform.position.y);
                if (isDown)
                {
                    Up();
                }

                else if (isTop)
                {
                    Down();
                }
            }

            if (finalLadder != null)
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
