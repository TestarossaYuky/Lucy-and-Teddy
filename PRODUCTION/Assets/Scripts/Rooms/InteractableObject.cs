using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{


    public int currentStage;
    public int currentRoom;

    private bool isUse = false;

    private CircleCollider2D waveCollider;
    private BoxCollider2D itemCollider;

    // Start is called before the first frame update
    void Start()
    {
        waveCollider = GetComponentInChildren<CircleCollider2D>();
        itemCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ActiveWave();
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            ActiveWave();
        }
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
            currentRoom = collision.GetComponent<Rooms>().GetRoomNb();
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public int GetStage()
    {
        return currentStage;
    }
}
