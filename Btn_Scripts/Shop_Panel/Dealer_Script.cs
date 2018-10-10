using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealer_Script : MonoBehaviour
{

    [SerializeField]
    GameObject tradePanel;

    readonly string[] replics = { "Привет", "А ты забавный, Пончику такие нрав..нравились" };
    private int replicsCount;
    private int currentReplicIndex;


    void Start()
    {
        replicsCount = replics.Length;
        currentReplicIndex = 0;
    }

    public string _Replic
    {
        get
        {
            return replics[currentReplicIndex];
        }
    }

    public void NewReplicPrepare()
    {
        if (currentReplicIndex < (replicsCount - 1))
        {
            currentReplicIndex++;
        }
        else
        {
            currentReplicIndex = 0;
        }
    }
    public void Interact()
    {
        tradePanel.SetActive(true);
    }
}
