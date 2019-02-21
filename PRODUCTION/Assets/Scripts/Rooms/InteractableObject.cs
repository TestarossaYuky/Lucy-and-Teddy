using System.Collections;
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



    private AudioSource mySource;

    public AudioClip RadioOn;
    public AudioClip RadioOff;

    public AudioClip RadioStation1;
    public AudioClip RadioStation2;

    public int isCheck = 0;

    [SerializeField]
    private string inputName;

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
        haveElectricity = myRooms.GetIsOn();

        if (haveElectricity)
        {
            
            if (inputName != null)
            {
                ChangeColor(spr.color);

                if (Input.GetKeyDown(inputName))
                {
                    isCheck = 0;
                    ActiveWave();
                    SetIsUse(true);
                    isCheck++;
                }

                if (Input.GetKeyUp(inputName))
                {
                    ActiveWave();

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

        if (isUse)
        {


            if(mySource != null) 
                mySource.mute = false;

            

            if (this.gameObject.name == "Radio")
            {
                if(isCheck > 0)
                {
                    StartCoroutine(PlayRadio());
                    isCheck = 0;
                }
            }
        }
        else
        {
            if(mySource != null)
            {
                mySource.mute = true;
                mySource.Stop();
                mySource.clip = null;
            }
        }
    }

    IEnumerator PlayRadio()
    {
        mySource.clip = RadioOn;
        mySource.PlayOneShot(RadioOn, 1f);
        yield return new WaitForSeconds(mySource.clip.length);

        mySource.clip = RadioStation1;
        mySource.PlayOneShot(RadioStation1, 0.05f);
        yield return new WaitForSeconds(mySource.clip.length);

        mySource.clip = RadioOff;
        mySource.PlayOneShot(RadioOff, 1f);
        yield return new WaitForSeconds(mySource.clip.length);

        isCheck = 0;
    }

    private void ChangeColor(Color c)
    {
        if(c != Color.white)
        {
            spr.color = Color.white;
            if(mySource != null)
            {
                mySource.clip = null;
                if (this.gameObject.name == "Radio")
                    isCheck = 0;
            }
                
           
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
