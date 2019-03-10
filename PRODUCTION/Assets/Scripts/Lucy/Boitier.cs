using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boitier : MonoBehaviour
{
    private List<bool> lastInput = new List<bool>() { false, false, false, false, false, false };

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
    #endregion

    #region MakeyMakey
    [SerializeField]
    private List<string> inputName = new List<string>();

    public List<bool> button = new List<bool>() { false, false, false, false, false, false };
    #endregion

    #region Overload

    private bool isOverload = false;
    private int overload = 0;

    #endregion

    private GameObject player;
    private PlayerMng myPlayer;

    public GameMng myGameMng;

    private void Awake()
    {
        globalGroup.Add(group1);
        globalGroup.Add(group2);
        globalGroup.Add(group3);
        globalGroup.Add(group4);
        globalGroup.Add(group5);
        globalGroup.Add(group6);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        else if (player != null)
            myPlayer = player.GetComponent<PlayerMng>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myPlayer != null)
        {
            if (myPlayer.GetStep2() != false && myPlayer.GetFirstLight() == true  ||myGameMng.currentState != GameMng.GameState.Tutorial)
            {
                if (isOverload)
                {
                    for (int i = 0; i < globalGroup.Count; i++)
                    {
                        Overload(globalGroup[i]);
                        button[i] = false;
                        lastInput[i] = false;

                    }
                    if (!Input.anyKey)
                        overload = 0;
                }

                else
                {
                    for (int i = 0; i < inputName.Count; i++)
                    {
                        if (Input.anyKeyDown)
                        {
                            if (Input.GetKeyDown(inputName[i]))
                            {
                                button[i] = true;

                                CheckRooms(globalGroup[i]);

                                overload++;
                            }

                            if (myPlayer.GetFirstLight() == true && myPlayer.GetClear() == false && myGameMng.currentState != GameMng.GameState.Tutorial)
                                myPlayer.SetClear(true);
                        }

                        if (Input.GetKeyUp(inputName[i]))
                        {

                            SwitchOnOff(button[i], globalGroup[i]);
                            button[i] = false;
                            lastInput[i] = button[i];

                            if (overload != 0)
                                overload--;
                        }

                    }
                }
                OverloadState();
            }
        }
        
        

    }

    #region Overload

    void Overload(List<Rooms> group)
    {
        
        for (int j = 0; j < group.Count; j++)
        {
            group[j].isOn = false;
            group[j].button = 0;
          
        }
       
    }

    void OverloadState()
    {
        if (overload > 3)
            isOverload = true;

        else
            isOverload = false;
    }

    #endregion

    #region Electricity

    void SwitchOnOff(bool active, List<Rooms> group)
    {
        if(!isOverload)
        {
            active = !active;

            if (active == true)
            {
                for (int i = 0; i < group.Count; i++)
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
    }

    void CheckRooms(List<Rooms> group)
    {
        for (int i = 0; i < button.Count; i++)
        {
            if (button[i] == true)
            {
                if(button[i] != lastInput[i])
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

    #endregion
}

