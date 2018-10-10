using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetAll_Script : MonoBehaviour {

    [SerializeField] GameObject parentPanel;
    [SerializeField] GameObject confirmPanel;
    string confirmText;

    void Start()
    {
        confirmText = "Вы точно хотите удалить весь прогресс?";
    }
    public void ResetConfirm()
    {
        foreach(Button btn in parentPanel.GetComponentsInChildren<Button>())
        {
            btn.interactable = false;
        }
        
        foreach (Button btn in confirmPanel.GetComponentsInChildren<Button>())
        {
            if (btn.name == "Yes_Btn")
            {
                btn.onClick.AddListener(() => Reset());
                break;
            }
        }
        foreach(Text text in confirmPanel.GetComponentsInChildren<Text>())
        {
            if(text.name=="Confirm_Text")
            {
                text.text = confirmText;
            }
        }
        confirmPanel.SetActive(true);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        confirmPanel.SetActive(false);
        foreach (Button gb in parentPanel.GetComponentsInChildren<Button>())
        {
            gb.interactable = true;
        }
    }
}
