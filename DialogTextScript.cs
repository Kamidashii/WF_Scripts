using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTextScript : MonoBehaviour
{
    public GameObject paper;
    public void SetText(string newtext)
    {
        GetComponent<Text>().text = newtext;
    }

    public void Start()
    {
        paper.SetActive(false);
    }
}
