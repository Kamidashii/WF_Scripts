using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    [SerializeField]
    public PolygonCollider2D[] colliders;
    public int currentColliderIndex = 0;

    public playerController player;

    // Use this for initialization

        public void SetColliderForSprite(int num)
    {
        colliders[currentColliderIndex].enabled = false;
        currentColliderIndex = num;
        colliders[currentColliderIndex].enabled = true;
    }

    public bool seen = false;
    public GameObject fire;
    public Transform shotSpawn;
    public float fireRate;
    public float fullReloading;
    public float waitVisiter;
    public GameObject Character;
    public Animator anim;

    public float maxHealth;
    public float health;
    public Slider healthSlider;

    public Collider2D danger;
    public int fireNum=0;

    public string playerFireTag;
    public string safetyTag;


    public bool startFire = false;

    IEnumerator BossActions()
    {
        anim.SetBool("Naughty", true);

        yield return new WaitForSeconds(waitVisiter);

        while (seen)
        {
            yield return StartCoroutine(CombatAction());
        }
        WaitForTarget();
    }



    IEnumerator CombatAction()
    {
            if (fireNum == 0)
            {
                anim.SetBool("Naughty", false);
                anim.SetBool("Shooting", true);
            }

        yield return StartCoroutine(SpawnFires());
            fireNum = 0;

            anim.SetBool("Shooting", false);
            anim.SetBool("Naughty", true);

            yield return new WaitForSeconds(fullReloading);

            anim.SetBool("Naughty", false);
    }

    void WaitForTarget()
    {
        anim.SetBool("Shooting", false);
        anim.SetBool("Naughty", false);
        anim.SetBool("Idle", true);
        startFire = false;
        healthSlider.value = health;
    }

    IEnumerator SpawnFires()
    {
        for (; fireNum < 10; fireNum++)
        {
            var bullet = Instantiate(fire, shotSpawn.position, shotSpawn.rotation);

            yield return new WaitForSeconds(bullet.GetComponent<FireMoveScript>().castingTime + 0.5f);
            if (!seen)
            {
                break;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collider2D)
    {
            danger = collider2D;
    }

    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D==danger)
        {
            danger = null;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (seen)
        {
            PlayerCombatEnable();

            CheckHit();

            if (!startFire)
            {
                startFire = true;
                StartCoroutine(BossActions());
            }
        }
        else
        {
            healthSlider.gameObject.SetActive(false);
        }
        healthSlider.value = health;
    }

    void PlayerCombatEnable()
    {
        healthSlider.gameObject.SetActive(true);
        player.DamageActive();
    }

    void CheckHit()
    {
        if (danger != null && danger.tag == playerFireTag)
        {
            health -= danger.gameObject.GetComponent<DangerObjectScript>().damage;
            danger.tag = safetyTag;
        }
    }
}
