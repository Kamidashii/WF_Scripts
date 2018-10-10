using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider_Script : MonoBehaviour {

    private Slider soundSlider;
	void Awake()
    {
        soundSlider = GetComponent<Slider>();
    }
	void Start ()
    {
        soundSlider.value = SoundSettings.soundLevel;
    }
	
	void Update ()
    {
        SoundSettings.soundLevel = soundSlider.value;
	}
}
