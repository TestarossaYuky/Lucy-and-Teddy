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

    private GameObject myPlayer;
    private GameObject myEnnemi;

    public GameObject playerPrefabs;
    public GameObject enemiPrefabs;

    public GameObject EXIT;

    public Transform SpawnTuto;
    public Transform SpawnFirst;
    public Transform SpawnSecond;

    public Transform SpawnEnemy1;
    public Transform SpawnEnemy2;

    [SerializeField]
    private GameObject Tutorial;

    [SerializeField]
    private GameObject Step1;

    [SerializeField]
    private GameObject Step2;


    // Start is called before the first frame update
    void Start()
    {
        //SetState(GameState.Tutorial);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameState.Tutorial:
                {
                    if (myPlayer == null)
                        myPlayer = Instantiate(playerPrefabs, SpawnTuto.transform.position, Quaternion.identity ,this.gameObject.transform);

                    EXIT.GetComponent<BoxCollider2D>().enabled = true;
                    break;
                }
            case GameState.First:
                {
                    Tutorial.SetActive(false);

                    if (myPlayer == null)
                        myPlayer = Instantiate(playerPrefabs, SpawnFirst.transform.position, Quaternion.identity, this.gameObject.transform);

                    if (myEnnemi == null)
                    {
                        EXIT.GetComponent<BoxCollider2D>().enabled = false;
                        myEnnemi = Instantiate(enemiPrefabs, SpawnEnemy1.transform.position, Quaternion.identity, this.gameObject.transform);
                    }
                        

                    break;
                }
            case GameState.Second:
                {
                    Tutorial.SetActive(false);
                    Step1.SetActive(false);
                    break;
                }
        }
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
    }

}
