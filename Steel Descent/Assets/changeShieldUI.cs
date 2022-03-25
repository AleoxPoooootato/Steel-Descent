using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeShieldUI : MonoBehaviour
{
    public Health script;

    Text shieldUI;

    void Awake(){
        shieldUI = GetComponent <Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        shieldUI.text = "Shields: " + script.currentShields + "\n";
    }
}
