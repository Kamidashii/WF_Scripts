using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyBtns_Panel : MonoBehaviour
{

    private playerController player;
    private int capacity;
    private Cell[] content;
    [SerializeField]
    private Button menuBtn;
    [SerializeField]
    Inventory inventory;
    private Button holeBtn;
    [SerializeField]
    private Text itemDescText;
    [SerializeField]
    ShopMenuBtns_Script menuBtnsPanel;

    void Awake()
    {
        holeBtn = inventory.holeBtn;
        player = FindObjectOfType<playerController>();
        GetCells();
    }

    void GetCells()
    {
        capacity = gameObject.transform.childCount;
        content = new Cell[capacity];
        int i = 0;
        foreach (Transform child in transform)
        {
            content[i] = child.gameObject.GetComponent<Cell>();
            i++;
        }
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                menuBtn.OnPointerDown(new PointerEventData(null));
                gameObject.SetActive(false);
            }
        }

        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != holeBtn.gameObject)
        {
            var item = EventSystem.current.currentSelectedGameObject.GetComponent<Item>();
            if (item != null)
            {
                itemDescText.text = item.descName + "\n" + item.desc + "\nЦена " + item.price.ToString();
            }
        }
        else
        {
            itemDescText.text = "";
        }
    }

    public void OnEnable()
    {
        menuBtnsPanel.MenuBtnsOf();
        player.inventory.ChangeUsable(Item.BtnActions.Buy);
        if (capacity > 0)
        {
            EventSystem.current.SetSelectedGameObject(content[0].gameObject.GetComponentInChildren<Button>().gameObject);
        }
    }


    public void OnDisable()
    {
        player.inventory.ChangeUsable(Item.BtnActions.Use);
        itemDescText.text = "";
    }
}
