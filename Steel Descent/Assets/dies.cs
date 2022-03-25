using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dies : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 1.0f;
    void Start()
    { 
        Destroy(gameObject, lifeTime);
    }


}
