using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boitier : MonoBehaviour
{
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

    bool m_wasActive = false;

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
        if(Input.GetKeyDown(inputName[0]))
        {

        }

        for (int i = 0 ; i < inputName.Count; i++)
            Electricity(globalGroup[i], inputName[i]);
        
    }

    void Electricity (List<Rooms> group, string x)
    {
        if(Input.GetKeyDown(x))
        {
            for (int i = 0; i < group.Count; i++)
            {
                group[i].isOn = true;
            }
            overload++;
        }

        if (Input.GetKeyUp(x))s
        {
            for (int i = 0; i < group.Count; i++)
            {
                group[i].isOn = false;
            }
            overload--;
        }
    }
}
