﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public bool isOn;
    public int button;

    [SerializeField]
    private int roomNb;
    
    private int stage;
    private string roomName;

    private Color baseColor;
    private SpriteRenderer spr;
    

    // Start is called before the first frame update
    void Start()
    {
        roomName = this.gameObject.name;
        spr = GetComponent<SpriteRenderer>();
        baseColor = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (!isOn)
            spr.color = baseColor;
        else
            spr.color = Color.white;
    }

    public int GetRoomNb()
    {
        return roomNb;
    }

    public bool GetIsOn()
    {
        return this.isOn;
    }

    public void SwitchLight(bool OnOff)
    {

        if (OnOff)
        {
            isOn = OnOff;
            button++;
          
        }

        else
        {
            button--;
            if (button == 0)
                isOn = OnOff;
        }

    }

}
