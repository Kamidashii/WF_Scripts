using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpeakPanel : MonoBehaviour {

    [SerializeField]
    private playerController player;
    [SerializeField]
    private Text dealerTextBlock;
    [SerializeField]
    Dealer_Script dealer;
    [SerializeField]
    private Button speakBtn;
    [SerializeField]
    ShopMenuBtns_Script menuBtnsPanel;
    
     void OnEnable()
    {
        player.inventory.ChangeUsable(Item.BtnActions.Use);
    }

    public void SetText()
    {
        dealerTextBlock.text = dealer._Replic;
        dealer.NewReplicPrepare();
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                speakBtn.OnPointerDown(new PointerEventData(null));
                dealerTextBlock.text = "";
                gameObject.SetActive(false);
            }
        }
    }

    
}
