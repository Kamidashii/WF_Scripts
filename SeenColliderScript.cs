using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeenColliderScript : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            gameObject.GetComponentInParent<BossScript>().seen = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            gameObject.GetComponentInParent<BossScript>().seen = false;
            gameObject.GetComponentInParent<BossScript>().startFire = false;
        }
    }
}
