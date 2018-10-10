using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string descName;
    public string desc;
    Inventory playerInventory;
    SellBtns_Panel shopInventory;
    public int price;
    GameControllerScript gameContoller;
    public bool isPlayerItem;
    public bool isProduct;
    public int id;
    private SellBtns_Panel sellPanel;
    private playerController player;
    private Button holeBtn;
    public enum BtnActions{ Buy=0,Sell,Use,Unknown}
    public BtnActions action=BtnActions.Unknown;

    void Start()
    {
        holeBtn = GameObject.FindGameObjectWithTag("ResetBtn").GetComponent<Button>();
        if(action==BtnActions.Unknown)
        {
            action = BtnActions.Use;
        }
        gameObject.GetComponent<Button>().onClick.AddListener(() => Pick());
        gameContoller = FindObjectOfType<GameControllerScript>();
        player = gameContoller.player.GetComponent<playerController>();
        playerInventory = gameContoller.playerInventory;
        
    }

    void SellItem()
    {
        sellPanel = FindObjectOfType<SellBtns_Panel>();
        gameContoller.gems += price;
        gameContoller.ChangeScore();
        playerInventory.Del_Item(gameObject);
        sellPanel.ResetProducts();
        sellPanel.SetItems();
    }

    void BuyItem()
    {
        gameContoller.gems -= price;
        gameContoller.ChangeScore();
        playerInventory.Add_Item(gameObject);
    }

    public void Pick()
    {
        if (action == BtnActions.Buy)
        {
            if (!isPlayerItem)
            {
                if (isProduct)
                {
                    if (gameContoller.gems >= price)
                    {
                        BuyItem();
                    }
                }
            }
        }

        else if(action==BtnActions.Sell)
        {
            if (isProduct)
            {
                if (isPlayerItem)
                {
                    SellItem();
                }
            }
        }

        else if(action==BtnActions.Use)
        {
            ;
        }
    }
    
}
