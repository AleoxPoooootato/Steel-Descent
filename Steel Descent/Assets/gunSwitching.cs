using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunSwitching : MonoBehaviour
{
    // Start is called before the first frame update
    public int selectedWeapon = 0;
    
    void Start()
    {
        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f){
            selectedWeapon += 1;
            if (selectedWeapon > 3){
                selectedWeapon = 0;
            }
            selectWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f){
            selectedWeapon -= 1;
            if (selectedWeapon < 0){
                selectedWeapon = 3;
            }
            selectWeapon();
        }
    }

    void selectWeapon(){
        int i = 0;
        foreach (Transform weapon in transform){
            if (i == selectedWeapon){
                weapon.gameObject.SetActive(true);
                Debug.Log("set active " + i);
            }
            else{
                weapon.gameObject.SetActive(false);
                Debug.Log("set inactive " + i);
            }
            i++;
        }
    }
}
