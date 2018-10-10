using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXLevel_Script : MonoBehaviour {

    private Slider fxSlider;
	void Awake()
    {
        fxSlider = GetComponent<Slider>();
    }
	
	void Start()
    {
        fxSlider.value = SoundSettings.fxLevel;
    }
	void Update ()
    {
        SoundSettings.fxLevel = fxSlider.value;
	}
}
