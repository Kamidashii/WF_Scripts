using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMover : MonoBehaviour
{
    float maxDistance;

    [SerializeField]
    float width;
    [SerializeField]
    float speed;
    [SerializeField]

    GameObject background;
    float current_x;
    bool flown;
    
    private float flownTime;

    void Start()
    {
        current_x = GetComponent<Transform>().position.x;
        flown = false;
        maxDistance = background.GetComponent<SpriteRenderer>().bounds.max.x*2+width;
    }
    void Update()
    {
        if (!flown)
        {
            if (GetComponent<Transform>().transform.position.x > maxDistance&&!flown)
            {
                Flown();
            }
            else
            {
                FirstFlyToBounds();
            }
        }
        else
        {
            FlyFromBoundsToBounds();
        }
    }

    void FirstFlyToBounds()
    {
        GetComponent<Transform>().position = new Vector3(Time.time * speed + current_x, 0f, 0f);
    }

    void Flown()
    {
        flownTime = Time.time;
        flown = true;
    }

    void FlyFromBoundsToBounds()
    {
        GetComponent<Transform>().position = new Vector3(Mathf.Repeat((Time.time - flownTime) * speed, maxDistance), transform.position.y, transform.position.z);
    }
}
