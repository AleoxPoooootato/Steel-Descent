using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrediction : MonoBehaviour
{
    public float bulletSpeed;
    private bool isHitting;
    public GameObject bulletOriginY;
    public GameObject bulletOriginX;
    public Vector3 bulletRotation;
    public Quaternion rotation;
    public int damage = 200;

    void Awake()
    {
        /*bulletOriginY = GameObject.Find("FPSPlayer");
        bulletOriginX = GameObject.Find("Camera");
        
        bulletRotation = new Vector3(
            bulletOriginX.transform.eulerAngles.x,
            bulletOriginY.transform.eulerAngles.y,
            0f
            );
        //Debug.Log("set angles to" + bulletOriginX.transform.rotation.x + ", " + bulletOriginY.transform.rotation.y + ", 0");
        transform.eulerAngles = bulletRotation;*/
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 Velocity = Vector3.forward * bulletSpeed * Time.deltaTime;
        //Quaternion rotation = Quaternion.Euler(transform.rotation);
        Debug.DrawLine(transform.position + transform.rotation * Velocity, transform.position, Color.black, 70/bulletSpeed);
        if (Physics.Raycast(transform.position, transform.rotation * Velocity, out hit, bulletSpeed * Time.deltaTime))
        {
            if (hit.transform.tag == "Enemy") 
            {
                hit.transform.gameObject.GetComponent<Health>().takeDamage(damage);
                Debug.Log("I did damage");
            }
            Debug.Log("I hit something");
            Destroy(gameObject);
        }
        transform.Translate(Velocity);
    }

   
}
