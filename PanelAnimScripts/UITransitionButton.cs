using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITransitionButton : MonoBehaviour {

    private UIWindow current;
    [SerializeField]
    private UIWindow next;

    public void Click()
    {
        next.isOpened = current.isOpened;
        current.isOpened = !current.isOpened;
    }

    void Awake()
    {
        current = GetComponentInParent<UIWindow>();
    }
}
