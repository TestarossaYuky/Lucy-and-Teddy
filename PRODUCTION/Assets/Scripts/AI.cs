using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    public enum State { Idle = 0, Move, Climb }

    [SerializeField]
    public enum Infiltration { Undetected, Detected, Trigger}

    [SerializeField]
    private State currentState;

    [SerializeField]
    private Infiltration currentInfiltration;

    private float currentDirection;
    private int currentFloor;
    private int currentRoom;

    private bool dirTrigger = false;
    private bool waitTrigger = true;

    private Vector3 currentPosition;

    private InteractableObject myObject;
    private int objectStage;
    private int objectRoom;

    private SpriteRenderer sprIcone;
    public Sprite trigger;
    public Sprite detect;

    private Transform View;

    private GameObject Lucy;

    #region Theme Song
    private AudioSource theme;
    [SerializeField]
    private AudioClip Main;
    [SerializeField]
    private AudioClip Trigger;
    [SerializeField]
    private AudioClip Detected;
    #endregion

    #region Song
    private AudioSource myAudio;

    [SerializeField]
    private AudioClip walk1;
    [SerializeField]
    private AudioClip walk2;

    [SerializeField]
    private AudioClip Enter;

    #endregion

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

    private GameObject START;
    private GameObject EXIT;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Move;
        currentInfiltration = Infiltration.Undetected;
        movement = Vector2.zero;
        currentDirection = 1;

        rgb2D = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
        sprIcone = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        View = this.transform.GetChild(0);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Lucy == null)
            Lucy = GameObject.FindGameObjectWithTag("Player");

        if(theme == null)
            theme = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        if (START == null)
            START = GameObject.Find("START");

        if (EXIT == null)
            EXIT = GameObject.Find("EXIT");

        MoveTo();
        Climb();
        
    }

    private void FixedUpdate()
    {
        if( Lucy != null && theme != null)
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

                        if ((currentFloor >= Lucy.GetComponent<PlayerMng>().GetCurrentStage() && Lucy.GetComponent<PlayerMng>().GetCurrentStage() >= currentFloor - 1) || (Lucy.GetComponent<PlayerMng>().GetCurrentStage() >= currentFloor && currentFloor >= Lucy.GetComponent<PlayerMng>().GetCurrentStage() - 1))
                        {
                            if (myAudio.isPlaying == false)
                            {
                                int random = Random.Range(0, 2);
                                if (random == 1)
                                {
                                    myAudio.PlayOneShot(walk1);
                                }
                                else if (random == 0)
                                    myAudio.PlayOneShot(walk2);
                            }
                        }



                        break;

                    }

                case State.Climb:
                    {

                        break;

                    }


            }


            switch (currentInfiltration)
            {
                case Infiltration.Detected:
                    {
                        if (myObject.GetIsUse() == false)
                        {
                            SetInfiltration(Infiltration.Undetected);
                        }

                        if (currentFloor == myObject.GetStage())
                        {
                            if (this.transform.position.x <= myObject.GetComponent<Transform>().position.x)
                            {
                                this.SetDirection(1);

                            }
                            else
                            {
                                this.SetDirection(-1);
                            }
                        }

                        else if (currentFloor > myObject.GetStage())
                        {
                            //check l'échelle la plus proche
                        }

                        sprIcone.sprite = detect;
                        sprIcone.enabled = true;

                        break;
                    }

                case Infiltration.Undetected:
                    {

                        sprIcone.enabled = false;
                        break;

                    }

                case Infiltration.Trigger:
                    {
                        sprIcone.sprite = trigger;
                        sprIcone.enabled = true;
                        break;

                    }


            }
            SpacialSong();
        }
        
    }

    void SetState(State state)
    {
        currentState = state;
    }

    public State GetState()
    {
        return currentState;
    }

    public void SetInfiltration(Infiltration infiltration)
    {
        currentInfiltration = infiltration;
        if(theme != null)
        {
            if (infiltration == Infiltration.Trigger)
            {
                theme.clip = Trigger;
                theme.Play();
                theme.loop = true;
            }

            else if (infiltration == Infiltration.Undetected)
            {
                theme.clip = Main;
                theme.Play();
                theme.loop = true;
            }

            else if (infiltration == Infiltration.Detected)
            {
                theme.Play();
                theme.loop = true;
            }
        }
        
    }

    public Infiltration GetInfiltration()
    {
        return currentInfiltration;
    }

    public void SetMyObject(InteractableObject obj)
    {
        myObject = obj;
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

        //Debug.Log("Current State: " + currentState.ToString());
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
        {
            this.sprRenderer.flipX = true;
            View.localScale = -this.transform.localScale;
        }
            

        else
        {
            this.sprRenderer.flipX = false;
            View.localScale = this.transform.localScale;
        }
            
        //Movement Script
        movement = new Vector2(speed * currentDirection, 0);
        this.rgb2D.velocity = movement * Time.deltaTime;
    }

    private void SpacialSong()
    {
        if(Lucy != null)
        {
            
            if(Lucy.transform.position.x > this.transform.position.x)
            {
               
                myAudio.panStereo = 1 * Lucy.transform.position.x + this.transform.position.x /4;
            }
            else if(Lucy.transform.position.x < this.transform.position.x)
            {

                myAudio.panStereo = -1 * Lucy.transform.position.x + this.transform.position.x /4;
            }
        }
    }

    public void SetDirection(int dir)
    {
        currentDirection = dir;
    }

    public int GetStage()
    {
        return currentFloor;
    }

    public float GetLadderChance()
    {
        return ladderChance;
    }

    public void SetLadderChance(float chance)
    {
        ladderChance = chance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "StartTrigger")
        {
            START.GetComponent<BoxCollider2D>().enabled = true;
            EXIT.GetComponent<BoxCollider2D>().enabled = true;
            SetState(State.Idle);
            myAudio.PlayOneShot(Enter);           
            StartCoroutine(WaitingTime(Enter.length));
            Destroy(collision.gameObject);
            
        }

        if(collision.tag == "Stage" && currentState != State.Climb)
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

        if (collision.tag == "Ladder" && currentInfiltration != Infiltration.Detected)
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

       

        if(currentInfiltration == Infiltration.Detected)
        {
            if (collision.tag == "Object")
            {
                if (collision.GetComponent<InteractableObject>().GetIsUse() == true)
                {
                    StartCoroutine(WaitingTime(0));
                    StartCoroutine(WaitingTime(1));
                    int test = Random.Range(0, 1);
                    if (test == 0)
                        SetDirection(1);
                    else
                        SetDirection(-1);

                    collision.GetComponent<InteractableObject>().SetIsUse(false);
                    SetInfiltration(Infiltration.Undetected);
                }

            }

            if (myObject.GetStage() > currentFloor)
            {

                if (collision.tag == "Ladder")
                {
                    canClimb = true;
                    collision.GetComponent<Ladder>().SetClimb(true);
                    ladderTransform = collision.transform;
                    if (isDown)
                    {
                        finalLadder = collision.gameObject.transform.GetChild(0).transform;
                    }
                }
            }

            else if (myObject.GetStage() < currentFloor)
            {
                if (collision.tag == "Ladder")
                {
                    canClimb = true;
                    collision.GetComponent<Ladder>().SetClimb(true);
                    ladderTransform = collision.transform;
                    if (isTop)
                    {
                        finalLadder = collision.gameObject.transform.GetChild(1).transform;
                    }
                }
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
        }

        if (collision.tag == "Stage" && currentState != State.Climb)
        {
            myStage = collision.GetComponent<Stage>();
            currentFloor = myStage.GetStage();

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
        
        if(currentState == State.Move && currentInfiltration != Infiltration.Detected)
        { 
            float waitingTime = Random.Range(3, 6);
            StartCoroutine(WaitingTime(waitingTime));
        }
    }

    IEnumerator WaitingTime(float time)
    {
        yield return new WaitForSeconds(time);

        
        if (currentState != State.Move)
            SetState(State.Move);
        else
            SetState(State.Idle);
    }

    private void Climb()
    {
        if (canClimb)
        {
            currentPosition.y = gameObject.transform.position.y;
     
            rgb2D.velocity = Vector2.zero;
            SetState(State.Climb);
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
