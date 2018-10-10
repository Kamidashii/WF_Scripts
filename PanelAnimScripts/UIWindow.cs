using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour {

    private Animator animator;
    private bool check = false;

    public bool isOpened
    {
        get
        {
            return animator.GetBool("isOpened");
        }
        set
        {
            if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
            if(value)
            {
                transform.SetAsFirstSibling();
            }
            animator.SetBool("isOpened", value);
        }
    }

    void OnEnable()
    {
        if (animator) return;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isOpened)
        {
            if (!check)
            {
                SetFirstNavi();
                check = true;
            }
        }
        else
        {
            check = false;
        }
    }

     public void SetFirstNavi()
    {
        Button current = GetComponentInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(current.gameObject);
    }
}
