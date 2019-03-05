using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMng : MonoBehaviour
{
    [SerializeField]
    private AudioClip Main;
    [SerializeField]
    private AudioClip Trigger;
    [SerializeField]
    private AudioClip Detected;

    private AudioSource myAudio;

    public AI myAi;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        SwitchSong(Main);
    }

    // Update is called once per frame
    void Update()
    {
        if(myAi != null)
        {
            
            if (myAi.GetInfiltration() == AI.Infiltration.Detected)
                SwitchSong(Detected);
                
            if (myAi.GetInfiltration() == AI.Infiltration.Undetected)
                SwitchSong(Main);

            if (myAi.GetInfiltration() == AI.Infiltration.Trigger)
                SwitchSong(Trigger);
        }
    }

    void SwitchSong(AudioClip clip)
    {
        
        myAudio.clip = clip;
        myAudio.Play();
        myAudio.loop = true;
    }
}
