using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraMoveScript : MonoBehaviour
{
    public GameObject Player;
    public Vector2 maxBounds;
    public Vector2 minBounds;
    [SerializeField]
    float cameraSpeed = 0.5f;

    public bool dying = false;
    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            if (!dying)
            {
                MoveToPlayer();
            }
        }
    }

    void MoveToPlayer()
    {
        var moveTo = new Vector3(

            Mathf.Clamp(
            Player.GetComponent<Transform>().position.x,
            minBounds.x, maxBounds.x),

            Mathf.Clamp(
            Player.GetComponent<Transform>().position.y,
            minBounds.y, maxBounds.y),

            GetComponent<Transform>().position.z);

        transform.position=Vector3.MoveTowards(transform.position, moveTo,cameraSpeed);
    }
}
