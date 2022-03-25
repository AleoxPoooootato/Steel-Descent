using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeHealthUI : MonoBehaviour
{
    public Health script;

    Text healthUI;

    void Awake(){
        healthUI = GetComponent <Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        healthUI.text = "Health: " + script.currentHealth;
    }
}

