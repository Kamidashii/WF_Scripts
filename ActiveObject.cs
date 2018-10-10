using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActiveObject : MonoBehaviour
{

    public string beforeText;
    public string findText;
    public string usingText;
    public string hintText;
    public string ifLockedHintText;
    public string[] randomReplicas;
    [SerializeField]
    string exitBookText;
    public int gems;
    public GameObject item;
    public bool isPick;
    
    public bool isOpen = true;

    
    [Serializable]
    public class Data
    {
        public int count = 0;
    }
    public Data data=new Data();

    public void Take()
    {
        if (isOpen)
        {
            if (data.count == 0)
            {
                FirstInter();
            }
            else if (data.count == 1)
            {
                SecondInter();
            }
            else
            {
                SetRandReplic();
            }
            if (data.count < int.MaxValue)
            {
                data.count++;
            }
        }
    }

    string SetNewLines(string text)
    {
        var strings = text.Split('/');
        text = "";
        foreach (string str in strings)
        {
            text += (str + "\n");
        }
        return text;
    }

    void FirstInter()
    {
        if (gameObject.tag == "Book")
        {
            SetDialog();
            hintText = exitBookText;
        }
        else
        {
            usingText = findText;
        }
    }

    void SetRandReplic()
    {
        if (randomReplicas.Length > 0)
        {
            usingText = randomReplicas[UnityEngine.Random.Range(0, randomReplicas.Length)];
        }
    }

    void SecondInter()
    {
        if (randomReplicas.Length > 0)
        {
            usingText = randomReplicas[UnityEngine.Random.Range(0, randomReplicas.Length)];
            usingText=SetNewLines(usingText);
        }
        gems = 0;
        if (gameObject.tag != "Book")
        {
            hintText = "";
        }
        if (isPick)
        {
            Destroy(gameObject);
        }
    }

    private void SetDialog()
    {
        usingText = findText;
        usingText=SetNewLines(usingText);
    }
    
    void Start()
    {
        if (data.count == 0)
        {
            usingText = beforeText;
        }
        else if (data.count == 1)
        {
            SetDialog();
            hintText = "";
        }
        else
        {
            if (gameObject.tag != "Book")
            {
                if (randomReplicas.Length > 0)
                {
                    SetRandReplic();
                }
                else
                {
                    usingText = "";
                }
            }
            else
            {
                SetDialog();
            }
            hintText = "";
            gems = 0;
        }
    }
}
