using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int capacity;
    public Cell[] content;
    [SerializeField]
    private Transform view;
    private int currentBtnIndex;
    [SerializeField]
    public Button holeBtn;
    private playerController player;
    [SerializeField]
    private string inventoryHint;

    private KeyCode[] keyCodes;
    public void Awake()
    {
        player = GameObject.FindObjectOfType<playerController>();

        keyCodes = new KeyCode[10] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };

        capacity = keyCodes.Length;
        content = new Cell[capacity];
        CreateCells();
    }

    void CreateCells()
    {
        for (int i = 0; i < capacity; ++i)
        {
            GameObject cell = Instantiate(Resources.Load<GameObject>("Cell"));
            cell.transform.SetParent(view);
            cell.transform.localScale = Vector2.one;
            cell.transform.position = view.position;
            content[i] = cell.GetComponent<Cell>();
            cell.name = string.Format("Cell [{0}]", i);
        }
    }

    public void ChangeUsable(Item.BtnActions action)
    {
        for (int i = 0; i < capacity; ++i)
        {
            if (content[i].transform.childCount != 0)
            {
                {
                    content[i].GetComponentInChildren<Item>().action=action;
                }
            }
        }
    }

    void Set_Hint()
    {
        player.gameController.hintText.text = inventoryHint;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ExitInventory();
            }
            else
            {
                for (int i = 0; i < keyCodes.Length; ++i)
                {
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        if (content[i].transform.childCount != 0)
                        {
                            PlayerBlock();
                            Set_Hint();
                            EventSystem.current.SetSelectedGameObject(content[i].GetComponentInChildren<Button>().gameObject);
                        }
                        break;
                    }
                }
            }
        }
    }


    void PlayerBlock()
    {
        player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, player.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        player.StopAnim();
        player.enabled = false;
    }

    void ExitInventory()
    {
        player.enabled = true;
        holeBtn.interactable = true;
        holeBtn.OnPointerDown(new UnityEngine.EventSystems.PointerEventData(null));
        holeBtn.interactable = false;
        player.gameController.ResetTexts();
    }
    public void Add_Item(GameObject item)
    {
        for (int i = 0; i < capacity; ++i)
        {
            if (content[i].transform.childCount == 0)
            {
                GameObject newItem = Instantiate(item);
                newItem.transform.SetParent(content[i].gameObject.transform);
                newItem.transform.position = content[i].transform.position;
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                newItem.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                newItem.GetComponent<Item>().isPlayerItem = true;
                var size = newItem.GetComponent<RectTransform>().rect.size;
                size= new Vector2(35,35);
                break;
            }
        }
    }

    public void Del_Item(GameObject item)
    {
        for (int i = 0; i < capacity; ++i)
        {
            if (content[i].transform.childCount != 0)
            {
                if (content[i].GetComponentInChildren<Item>().id == item.GetComponentInChildren<Item>().id)
                {
                    foreach (Transform btn in content[i].transform)
                    {
                        Destroy(btn.gameObject);
                    }
                    break;
                }
            }
        }
    }
}
