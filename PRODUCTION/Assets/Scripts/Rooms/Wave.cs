﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private AI myAI;

    //// Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
    
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.gameObject.tag == "Ennemi")
        {
            AI myAI = collision.gameObject.GetComponent<AI>();
            myAI.SetMyObject(GetComponentInParent<InteractableObject>());
            myAI.SetInfiltration(AI.Infiltration.Detected);
        }
    }

    //void ActiveSelf()
    //{
    //    if (isActive == false)
    //    {
    //        this.GetComponent<CircleCollider2D>().enabled = true;
    //        myStage = myInteract.GetStage();
    //        this.GetComponentInParent<InteractableObject>().SetIsUse(true);
    //        isActive = true;

    //    }

    //    else
    //    {
    //        this.GetComponent<CircleCollider2D>().enabled = false;
    //        isActive = false;
    //    }

    //}

    //public InteractableObject GetInteract()
    //{
    //    return this.myInteract;
    //}

    //public int GetStage()
    //{
    //    return myStage;
    //}

    //public bool GetIsActive()
    //{
    //    return isActive;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Ladder")
    //    {
    //        Ladder ladder = collision.GetComponent<Ladder>();
    //    }


    //}
}
