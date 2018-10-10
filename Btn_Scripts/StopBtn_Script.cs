using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBtn_Script : MonoBehaviour {

    public bool isPaused;

    public void Start()
    {
        if (Time.timeScale != 0f)
        {
            isPaused = false;
        }
    }
    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
        }

    }
}
