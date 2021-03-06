﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public int currentStage;
    public int currentRoom;

    private Rooms myRooms;

    public bool haveElectricity = false;
    public bool isUse = false;

    private CircleCollider2D waveCollider;
    private BoxCollider2D itemCollider;
    private SpriteRenderer spr;
    private Color baseColor;

    [SerializeField]
    private string myColor;

    private AudioSource mySource;

    public AudioClip RadioOn;
    public AudioClip RadioOff;

    public AudioClip RadioStation1;
    public AudioClip RadioStation2;


    public AudioClip PhonoOn;
    public AudioClip PhonoOff;

    public AudioClip PhonoStation1;
    public AudioClip PhonoStation2;

    public int isCheck = 0;

    [SerializeField]
    private string input;

    public GameMng myGameMng;

    private PlayerMng myPlayer;

    private bool isFirstListen = false;

    // Start is called before the first frame update
    void Start()
    {
        waveCollider = GetComponentInChildren<CircleCollider2D>();
        itemCollider = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        baseColor = spr.color;
        mySource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer == null && GameObject.FindGameObjectWithTag("Player") != null)
            myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMng>();

        else if(myPlayer != null)
        {
            haveElectricity = myRooms.GetIsOn();
            if (myPlayer.GetJack() != false || myGameMng.currentState != GameMng.GameState.Tutorial)
            {
                if (haveElectricity)
                {

                    if (input != null)
                    {
                        ChangeColor(spr.color);

                        if (Input.GetKeyDown(input))
                        {
                            isCheck = 0;
                            ActiveWave();
                            SetIsUse(true);
                            isCheck++;
                        }

                        if (Input.GetKeyUp(input))
                        {
                            ActiveWave();
                            if (mySource != null && mySource.isPlaying == true)
                                mySource.Stop();
                        }
                    }
                }
                else
                {
                    spr.color = baseColor;
                    SetIsUse(false);
                    isCheck = 0;

                    waveCollider.enabled = false;
                }
            }
            if (isUse)
            {


                if (mySource != null)
                    mySource.mute = false;



                if (this.gameObject.name == "Radio")
                {
                    if (isCheck > 0)
                    {
                        StartCoroutine(PlayRadio());
                        isCheck = 0;
                    }
                }
                else if (this.gameObject.name == "Phonographe")
                {
                    if (isCheck > 0)
                    {
                        StartCoroutine(PlayPhonographe());
                        isCheck = 0;
                    }
                }
            }
            else
            {
                if (mySource != null)
                {
                    mySource.mute = true;
                    mySource.Stop();
                    mySource.clip = null;
                }
            }
        }

       
        

        
    }

    public bool GetListen()
    {
        return isFirstListen;
    }

    IEnumerator PlayRadio()
    {
        mySource.clip = RadioOn;
        mySource.PlayOneShot(RadioOn, 1f);
        yield return new WaitForSeconds(mySource.clip.length);

        int i = Random.Range(0, 2);
 
            if (i == 0)
            {
                mySource.clip = RadioStation1;
                mySource.PlayOneShot(RadioStation1, 0.05f);
                yield return new WaitForSeconds(mySource.clip.length);
            }

            else if (i == 1)
            {
                mySource.clip = RadioStation2;
                mySource.PlayOneShot(RadioStation2, 0.05f);
                yield return new WaitForSeconds(mySource.clip.length);
            }



            mySource.clip = RadioOff;
            mySource.PlayOneShot(RadioOff, 1f);
            yield return new WaitForSeconds(mySource.clip.length);

            isCheck = 0;
      
        
    }

    IEnumerator PlayPhonographe()
    {
        mySource.clip = PhonoOn;
        mySource.PlayOneShot(PhonoOn, 1f);
        yield return new WaitForSeconds(mySource.clip.length);

        int i = Random.Range(0, 2);

        if (isFirstListen == false)
        {
            mySource.clip = PhonoStation1;
            mySource.PlayOneShot(PhonoStation1, 0.05f);
            yield return new WaitForSeconds(1.5f);
            mySource.Stop();
            isCheck = 0;
            isFirstListen = true;
        }
        else
        {
            if (i == 0)
            {
                mySource.clip = PhonoStation1;
                mySource.PlayOneShot(PhonoStation1, 0.05f);
                yield return new WaitForSeconds(mySource.clip.length);
            }

            else if (i == 1)
            {
                mySource.clip = PhonoStation2;
                mySource.PlayOneShot(PhonoStation2, 0.05f);
                yield return new WaitForSeconds(mySource.clip.length);
            }



            mySource.clip = PhonoOff;
            mySource.PlayOneShot(PhonoOff, 1f);
            yield return new WaitForSeconds(mySource.clip.length);

            isCheck = 0;
        }

       
    }


    private void ChangeColor(Color c)
    {
        if(c != Color.white && myColor == "White")
        {
            spr.color = Color.white;
            if(mySource != null)
            {
                mySource.clip = null;
                if (this.gameObject.name == "Radio")
                    isCheck = 0;
            }
        }

        if(myColor == "Red")
        {
            spr.color = Color.red;
        }

        else if(myColor == "Blue")
        {
            spr.color = Color.blue;
        }

        else if (myColor == "Green")
        {
            spr.color = Color.green;
        }

        else if (myColor == "Yellow")
        {
            spr.color = Color.green;
        }
    }

    private void FixedUpdate()
    {
        
    }

    void ActiveWave()
    {
        if (waveCollider.enabled == false)
            waveCollider.enabled = true;
        else
            waveCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Stage")
        {
            currentStage = collision.GetComponent<Stage>().GetStage();
        }

        if (collision.tag == "Rooms")
        {
            myRooms = collision.GetComponent<Rooms>();
            currentRoom = myRooms.GetRoomNb();
            
        }
    }


    public int GetRoom()
    {
        return currentRoom;
    }

    public bool GetIsUse()
    {
        return isUse;
    }

    public void SetIsUse(bool x)
    {
        isUse = x;
    }

    public int GetStage()
    {
        return currentStage;
    }
}
