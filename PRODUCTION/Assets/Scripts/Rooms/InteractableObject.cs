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

    // Start is called before the first frame update
    void Start()
    {
        waveCollider = GetComponentInChildren<CircleCollider2D>();
        itemCollider = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        haveElectricity = myRooms.GetIsOn();
        if (haveElectricity)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ActiveWave();
                SetIsUse(true);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                ActiveWave();
            }

        }
        else
        {
            SetIsUse(false);
         
            waveCollider.enabled = false;
        }

        if (isUse)
        {
            spr.color = Color.red;
        }
        else
            spr.color = Color.white;
       
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
