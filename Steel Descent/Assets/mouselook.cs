using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class mouselook : MonoBehaviour
{
    public Transform pb;
    
    public float mouseSens = 10000f;
    public float amountMovedVertical = 0f;
    public float xRot = 0f;
    public bool recoiling = false;
    public float mouseMovedRecoil = 0f;
    public float mouseYBeforeRecoil;
    public float resetTarget = 10;
    public LayerMask interactableMask;
    public float interactionProgress = 0;
    public float posToRotateTo = 0f;
    public float snappiness = 0.15f;
    public float waitAfterStopFiring = 0.3f;
    public float waitAfterStopFiringProgress = 0f;
    public Image interactionProgressBar;
    public bool interactionPressed;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (posToRotateTo == 0){
            waitAfterStopFiringProgress += Time.deltaTime;
        }
        else{
            waitAfterStopFiringProgress = 0;
        }
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        if (Mathf.Abs(posToRotateTo) > 0.02f){
            transform.localRotation = Quaternion.Euler(xRot += Mathf.Lerp(0, posToRotateTo, snappiness), 0f, 0f);
            posToRotateTo -= posToRotateTo * snappiness;
        }
        else{
            posToRotateTo = 0;
        }
        if(recoiling)
        {
            mouseMovedRecoil += mouseY;
            amountMovedVertical += mouseY;
            if (mouseMovedRecoil < 0)
            {
                mouseMovedRecoil = 0;
            }
            if (amountMovedVertical < 0)
            {
                amountMovedVertical = 0;
            }
        }
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        //pb.Rotate(Vector3.up * mouseX);
        /*if (!recoiling){
            transform.localRotation = Quaternion.Euler(xRot -= resetTarget / 9, 0f, 0f);
            resetTarget -= resetTarget / 9;
        }*/
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, interactableMask))
        {
            Debug.Log("interactable in range");
            if (Input.GetKeyDown(KeyCode.E)){
                interactionPressed = true;
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (!hit.transform.gameObject.GetComponent<InteractionBehavior>().interactionComplete && interactionPressed)
                {
                    Debug.Log("interacting!");
                    interactionProgress += Time.deltaTime;
                    hit.transform.gameObject.GetComponent<InteractionBehavior>().changeProgress(interactionProgress);
                    interactionProgressBar.fillAmount = interactionProgress / hit.transform.gameObject.GetComponent<InteractionBehavior>().threshold;
                }
                else{
                    interactionProgress = 0;
                    interactionProgressBar.fillAmount = 0;
                    interactionPressed = false;
                    hit.transform.gameObject.GetComponent<InteractionBehavior>().interactionComplete = false;
                }
            }
            else{
                interactionProgress = 0;
                interactionProgressBar.fillAmount = 0;
            }
        }
        else if (interactionProgress > 0){
            interactionProgress = 0;
            interactionProgressBar.fillAmount = 0;
        }
    }

    public void resetMouseVertical()
    {
        /*if (mouseMovedRecoil > amountMovedVertical && mouseMovedRecoil > 0){
            //resetTarget = xRot += amountMovedVertical
            transform.localRotation = Quaternion.Euler(xRot += amountMovedVertical, 0f, 0f);
        }
        else if (mouseYBeforeRecoil > xRot){
            //transform.localRotation = Quaternion.Euler(xRot += (amountMovedVertical - mouseMovedRecoil), 0f, 0f);
            //resetTarget = xRot += mouseYBeforeRecoil - xRot
            transform.localRotation = Quaternion.Euler(xRot += mouseYBeforeRecoil - xRot, 0f, 0f);
        }*/

        //transform.localRotation = Quaternion.Euler(xRot += mouseMovedRecoil - amountMovedVertical, 0f, 0f);
        posToRotateTo += mouseMovedRecoil - amountMovedVertical;
        amountMovedVertical = 0f;
        mouseMovedRecoil = 0f;
        recoiling = false;
    }
    
    public void rotateMouseVertical(float amount)
    {
        if (recoiling == false){
            mouseYBeforeRecoil = xRot;
        }
        posToRotateTo -= amount;
        //transform.localRotation = Quaternion.Euler(xRot -= amount, 0f, 0f); // the old implementation, instant flick
        mouseMovedRecoil += amount;
        //amountMovedVertical += amount;
        recoiling = true;
    }

    /*private IEnumerator (float waitTime){
        yield return new WaitForSeconds(waitTime);
        
    }*/
}
