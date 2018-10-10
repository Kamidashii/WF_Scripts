using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToOptions_Btn : MonoBehaviour {

    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject parentPanel;
	public void ToOptions()
    {
        
        optionsPanel.SetActive(true);
        parentPanel.SetActive(false);
    }
}
