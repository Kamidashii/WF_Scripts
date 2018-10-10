using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class No_Btn : MonoBehaviour {

    [SerializeField] GameObject parentPanel;
    [SerializeField] GameObject menuPanel;

    public void GoBack()
    {
        parentPanel.SetActive(false);
        foreach(Button gb in menuPanel.GetComponentsInChildren<Button>())
        {
            gb.interactable = true;
        }
    }
}
