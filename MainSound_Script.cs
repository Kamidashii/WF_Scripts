using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSound_Script : MonoBehaviour {

    [SerializeField]
    private AudioSource soundtrack;

    void Start()
    {
        //DontDestroyOnLoad(soundtrack);
    }

    void Update()
    {
        soundtrack.volume = SoundSettings.soundLevel;
    }
}
