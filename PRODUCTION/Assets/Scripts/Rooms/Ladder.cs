using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private float aiChance = 0.7f;

    [SerializeField]
    private float secondChance = 0.2f;

    private bool first = false;
    private bool climb = false;
    private float baseChance;
    private List<int> currentStage;

    // Start is called before the first frame update
    void Start()
    {
        baseChance = aiChance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public float GetAiChance()
    {
        return this.aiChance;
    }

    public void SetAiChance(float chance)
    {
        aiChance = chance;
    }

    public void SetClimb(bool x)
    {
        climb = x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Ennemi")
        {
            if(first)
            {
                SetAiChance(baseChance);
                first = false;
                climb = false;
            } 
        }
        if (collision.tag == "Stage")
        {
            currentStage.Add(collision.GetComponent<Stage>().GetStage());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ennemi")
        {
            if (!first)
            {
                if (climb)
                {
                    SetAiChance(secondChance);
                }
            }
        }
    }
}
