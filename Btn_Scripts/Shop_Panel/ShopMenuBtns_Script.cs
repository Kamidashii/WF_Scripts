using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopMenuBtns_Script : MonoBehaviour {
    [SerializeField]
    GameObject buyPanel;
    [SerializeField]
    GameObject sellPanel;
    [SerializeField]
    GameObject speakPanel;
    [SerializeField]
    GameObject activeObject;
    [SerializeField]
    playerController player;
    [SerializeField]
    Button buyBtn, sellBtn, speakBtn;

    [SerializeField]
    string buyHint, sellHint, speakHint,menuHint;

    GameControllerScript gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        EventSystem.current.SetSelectedGameObject(sellBtn.gameObject);
    }

	void Update ()
    {
		if(Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                if (!sellPanel.activeInHierarchy && !buyPanel.activeInHierarchy && !speakPanel.activeInHierarchy)
                {
                    gameController.ResetTexts();
                    gameObject.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    MenuBtnsOn();
                }
            }
        }
	}

    public void ShowBuy()
    {
        sellPanel.SetActive(false);
        buyPanel.SetActive(true);
        gameController.hintText.text = buyHint;
    }
    public void ShowSell()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);
        gameController.hintText.text = sellHint;
    }

    public void ShowSpeak()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(false);
        speakPanel.SetActive(true);
        gameController.hintText.text = speakHint;
        speakPanel.GetComponent<SpeakPanel>().SetText();
    }

    public void MenuBtnsOf()
    {
        buyBtn.interactable = false;
        sellBtn.interactable = false;
        speakBtn.interactable = false;
    }

    void OnEnable()
    {
        gameController.hintText.text = menuHint;
    }
    public void MenuBtnsOn()
    {
        gameController.hintText.text = menuHint;
        buyBtn.interactable = true;
        sellBtn.interactable = true;
        speakBtn.interactable = true;
    }

}
