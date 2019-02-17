using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    public int stage;
    

    private int nbRooms;

    // Start is called before the first frame update
    void Start()
    {
       foreach(Transform child in transform)
        {
            nbRooms += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetStage() { return stage; }
    public int GetNbRooms() { return nbRooms; }



    
}
