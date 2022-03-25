using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDeath : MonoBehaviour
{
    public Health mobHealth;
    public GameObject mob;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mobHealth.currentHealth <= 0){
            Destroy(mob);
        }
    }
}
