using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject mainBullet;
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Ground":
                Debug.Log("I hit the ground");
                Destroy(mainBullet);
                break;
            default:
                Debug.Log("I hit not the ground");
                break;
        }
        Debug.Log("I hit something");
    }
}
