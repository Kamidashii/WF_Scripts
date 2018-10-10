using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackFromOptions : MonoBehaviour {

    [SerializeField]
    GameObject parentPanel;
    [SerializeField]
    GameObject menuPanel;

    public void GoBack()
    {
        parentPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
