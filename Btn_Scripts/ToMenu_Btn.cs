using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ToMenu_Btn : MonoBehaviour {
    private GameObject gameController;
    
    void Start()
    {
        gameController = GameObject.Find("GameController");
    }
    public void MenuLoader()
    {
        Debug.Log("Button was down!");

        gameController.GetComponent<GameControllerScript>().SaveGame();

        SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Single);
    }
}
