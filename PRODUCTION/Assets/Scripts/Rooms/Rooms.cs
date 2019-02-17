using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField]
    public Sprite Light;

    [SerializeField]
    public Sprite Dark;


    
    public bool isOn;
    public int button;

    [SerializeField]
    private int roomNb;
    

    private int stage;
    private string roomName;

    private SpriteRenderer spr;
    

    // Start is called before the first frame update
    void Start()
    {
        roomName = this.gameObject.name;
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSprite();
    }

    void ChangeSprite()
    {
        if (!isOn)
            spr.sprite = Dark;
        else
            spr.sprite = Light;
    }

    public int GetRoomNb()
    {
        return roomNb;
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
