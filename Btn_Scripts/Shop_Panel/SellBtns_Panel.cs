using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellBtns_Panel : MonoBehaviour
{

    [SerializeField]
    Inventory inventory;
    private int capacity;
    private Cell[] content;
    [SerializeField]
    private Transform view;
    [SerializeField]
    private Button menuBtn;
    private Button holeBtn;
    private playerController player;
    [SerializeField]
    Text itemDescText;
    [SerializeField]
    ShopMenuBtns_Script menuBtnsPanel;

    public void Awake()
    {
        player = FindObjectOfType<playerController>();
        holeBtn = inventory.holeBtn;
        capacity = inventory.content.Length;
        content = new Cell[capacity];
        CreateCells();
    }

    public void OnEnable()
    {
        menuBtnsPanel.MenuBtnsOf();
        player.inventory.ChangeUsable(Item.BtnActions.Sell);
        SetItems();
        for (int i = 0; i < capacity; ++i)
        {
            if (content[i].transform.childCount != 0)
            {
                EventSystem.current.SetSelectedGameObject(content[i].GetComponentInChildren<Button>().gameObject);
                break;
            }
        }
    }


    public void OnDisable()
    {
        player.inventory.ChangeUsable(Item.BtnActions.Use);
        ResetProducts();
        itemDescText.text = "";
    }

    public void ResetProducts()
    {
        for (int i = 0; i < capacity; ++i)
        {
            foreach (Transform btn in content[i].transform)
            {
                Destroy(btn.gameObject);
            }
        }
    }

    private void CreateCells()
    {
        for (int i = 0; i < capacity; ++i)
        {
            GameObject cell = Instantiate(Resources.Load<GameObject>("Cell"));
            cell.transform.SetParent(view);
            cell.transform.position = view.transform.position;
            cell.transform.localScale = Vector2.one;
            content[i] = cell.GetComponent<Cell>();
            cell.name = "SellCell: " + i;
        }
    }

    public void SetItems()
    {
        bool checkItem = false;
        
        for (int i = 0; i < capacity; ++i)
        {
            if (inventory.content[i].transform.childCount > 0)
            {
                Button item = Instantiate(inventory.content[i].GetComponentInChildren<Button>());
                item.transform.SetParent(content[i].transform);
                item.transform.position = content[i].transform.position;
                item.transform.localScale = Vector3.one;
                item.GetComponent<RectTransform>().offsetMin = Vector3.zero;
                item.GetComponent<RectTransform>().offsetMax = Vector3.zero;
                if (item.isActiveAndEnabled && !checkItem)
                {
                    EventSystem.current.SetSelectedGameObject(item.gameObject);
                    checkItem = true;
                }
            }
        }
    }

        void Update()
        {
            if (Input.anyKey)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    gameObject.SetActive(false);
                    menuBtn.OnPointerDown(new PointerEventData(null));
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

    }
