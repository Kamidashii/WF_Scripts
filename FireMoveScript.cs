using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FireMoveScript : MonoBehaviour {

    // Use this for initialization

    public float speed;
    public float distance;
    public Vector2 direction;
    public float castingTime;
    public Animator anim;
    private float flown;
    private bool ready = false;
    public float xSpawn;

    IEnumerator Caster()
    {
        yield return new WaitForSeconds(castingTime);
        ready = true;
    }

	void Start ()
    {
        flown = 0f;
        gameObject.GetComponent<Collider2D>().isTrigger = false;
        StartCoroutine("Caster");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ready)
        {
            flown = GetComponent<Transform>().position.x - xSpawn;

            if (flown > 0)
            {
                StartFly();
            }

            if (flown < distance)
            {
                ContinueFly();
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}

    void StartFly()
    {
        anim.SetBool("ready", ready);
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    void ContinueFly()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
}
