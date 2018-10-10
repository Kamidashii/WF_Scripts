using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChangeScript : MonoBehaviour {
    

    public GameControllerScript gameController;
	public void ChangeScore(int value)
    {
       GetComponent<Text>().text = "Собрано " + gameController.gems + " драгоценных камней из " + gameController.maxGems;
    }
}
