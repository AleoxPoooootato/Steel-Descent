using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehavior : MonoBehaviour
{
    public float threshold;
    public string actionType;
    public bool interactionComplete;
    public MineralEvent mineralScript;

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeProgress(float progress){
        if(progress >= threshold){
            doAction(actionType);
        }
    }

    void doAction(string action){
        interactionComplete = true;
        if(action == "default"){
            Debug.Log("interaction complete");
        }
        if(action == "mineral"){
            mineralScript.SpawnWave();
        }
    }
}
