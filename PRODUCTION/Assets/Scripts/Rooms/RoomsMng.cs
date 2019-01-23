using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsMng : MonoBehaviour
{
    public static RoomsMng instance;

    public string currentRoom;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
