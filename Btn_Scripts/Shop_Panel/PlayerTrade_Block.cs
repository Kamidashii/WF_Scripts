using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrade_Block : MonoBehaviour {

    private playerController player;
    private Inventory inventory;
    void Awake()
    {
        player = GameObject.FindObjectOfType<playerController>();
        inventory = GameObject.FindObjectOfType<Inventory>();
    }

	void OnEnable()
    {
        inventory.gameObject.SetActive(false);
        if (player)
        {
            player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, player.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            player.StopAnim();
            player.enabled = false;
        }
    }

    void OnDisable()
    {
        inventory.gameObject.SetActive(true);
        if (player)
        {
            player.enabled = true;
        }
    }
}
