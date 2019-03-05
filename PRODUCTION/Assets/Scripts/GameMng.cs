using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public enum GameState
    {
        Tutorial = 0,
        First,
        Second,
        Third,
    }

    public GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        SetState(GameState.Tutorial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
    }

}
