using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    private string roomName;
    public bool isOn;
    public int button;
    private int stage;

    // Start is called before the first frame update
    void Start()
    {
        roomName = this.gameObject.name;
   
    }

    // Update is called once per frame
    void Update()
    {

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
