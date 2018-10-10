using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsTextScript : MonoBehaviour {
    
    public void SetText(string newtext)
    {
        GetComponent<Text>().text = newtext;
    }

}
