using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{

    public enum GameState { Tutorial, Step1, Step2};

    public GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
