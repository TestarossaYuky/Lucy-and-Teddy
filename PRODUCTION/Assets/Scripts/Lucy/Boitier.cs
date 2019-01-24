using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boitier : MonoBehaviour
{

    private bool first = false;

    private List<bool> lastInput = new List<bool>() { false, false, false, false, false, false };

    private int overload = 0;
   
    private List<List<Rooms>> globalGroup = new List<List<Rooms>>();

    #region List Rooms
    [SerializeField]
    private List<Rooms> group1 = new List<Rooms>();

    [SerializeField]
    private List<Rooms> group2 = new List<Rooms>();

    [SerializeField]
    private List<Rooms> group3 = new List<Rooms>();

    [SerializeField]
    private List<Rooms> group4 = new List<Rooms>();

    [SerializeField]
    private List<Rooms> group5 = new List<Rooms>();

    [SerializeField]
    private List<Rooms> group6 = new List<Rooms>();

    [SerializeField]
    private List<string> inputName = new List<string>();
    #endregion

    public List<bool> button = new List<bool>() { false, false, false, false, false, false };

    // Start is called before the first frame update
    void Start()
    {
        globalGroup.Add(group1);
        globalGroup.Add(group2);
        globalGroup.Add(group3);
        globalGroup.Add(group4);
        globalGroup.Add(group5);
        globalGroup.Add(group6);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < inputName.Count; i++)
        {
            if (Input.GetKeyDown(inputName[i]))
            { 
                button[i] = true;
                CheckRooms(globalGroup[i]);
            }

            if (Input.GetKeyUp(inputName[i]))
            {
                SwitchOnOff(button[i], globalGroup[i]);
                button[i] = false;
                lastInput[i] = button[i];
            }
        }
    }

    void SwitchOnOff(bool active, List<Rooms> group)
    {
        active = !active;

        if(active == true)
        {
            for (int i=0; i < group.Count; i++)
            {
                group[i].SwitchLight(true);
            }
        }

        else
        {
            for (int i = 0; i < group.Count; i++)
            {
                group[i].SwitchLight(false);
            }
           
        }
    }

    void CheckRooms(List<Rooms> group)
    {
        for (int i = 0; i < button.Count; i++)
        {
            if (button[i] == true)
            {
                if(lastInput[i] != button [i])
                { 
                    SwitchOnOff(false, group);
                    lastInput[i] = button[i];
                }   
            }

            else
            {
                for (int j = 0; j < group.Count; j++)
                {
                    if (group[j].isOn == true && group[j].button > 1)
                        group[j].isOn = true;
                }
            }
        }
    }
}
