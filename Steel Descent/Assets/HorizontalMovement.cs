using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float mouseX;
    public float yRot;
    public float mouseSens = 1000f;

    // Update is called once per frame
    void Update()
    {
        //changes y rotation of the player body based on mouse movement
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        yRot += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);
    }
}