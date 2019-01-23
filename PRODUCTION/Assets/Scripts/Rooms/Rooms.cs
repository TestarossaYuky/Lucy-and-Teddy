using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    private string roomName;
    public bool isOn;
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

    void Light(bool on)
    {
        if(on)
        {

        }
    }
}
