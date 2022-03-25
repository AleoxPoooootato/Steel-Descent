using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public Transform bulletOrigin;
    public float range = 100;
    public GameObject coob;
    public float fireAfterburn = 1f;
    public bool isFiring;
    public bool isAuto = true;
    public float spreadModifier1 = 0.5f;
    public bool isProjectile = false;
    public float bulletSpeed = 10;
    public float delayBeforeFireTime;
    public float secDelayBeforeFire = 0.5f;
    public bool isPassedDelay;
    public float[] recoilArrayY;
    public float[] recoilArrayX;
    public int recoilPos = 0;
    private mouselook mouseMove;
    public Quaternion towardsTarget;
    public V2move characterMove;
    private float xRecoil;
    public float totalXMove;
    public bool isReloading = false;
    public int currentAmmo;
    public int maxAmmo;
    public float reloadProgress;
    public float reloadTime;
    public Image reloadProgressBar;
    
    void Awake()
    {
        mouseMove = bulletOrigin.GetComponent<mouselook>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReloading){
            isReloading = true;
        }
        if(Input.GetButton("Fire1") && !isReloading)
        {
            delayBeforeFireTime += Time.deltaTime;
            if(delayBeforeFireTime >= secDelayBeforeFire)
            {
                isPassedDelay = true;
            }
        }
        else
        {
            if (isPassedDelay && mouseMove.waitAfterStopFiringProgress > mouseMove.waitAfterStopFiring)
            {
                mouseMove.resetMouseVertical();
                characterMove.rotateHorizontally(-totalXMove);
                totalXMove = 0;
                delayBeforeFireTime = 0;
                isPassedDelay = false;
                recoilPos = 0;
            }
        }
        if(isReloading){
            reloadProgress += Time.deltaTime;
            reloadProgressBar.fillAmount = reloadProgress / reloadTime;
            if(reloadProgress >= reloadTime){
                isReloading = false;
                currentAmmo = maxAmmo;
                reloadProgress = 0;
                reloadProgressBar.fillAmount = 0;
            }
        }
        else
        {
            if(isPassedDelay)
            {
                if (totalXMove != 0)
                {
                    totalXMove += (-totalXMove / Mathf.Abs(totalXMove)) * Mathf.Abs(characterMove.mouseX);
                }
                if (Input.GetButtonDown("Fire1") && isFiring == false || Input.GetButton("Fire1") && isFiring == false && isAuto == true)
                {
                    isFiring = true;
                    shoot();
                    recoilPos += 1;
                    Invoke("resetShot", fireAfterburn);
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    if (isAuto == true)
                    {
                        isAuto = false;
                    }
                    else
                    {
                        isAuto = true;
                    }
                }
            }
        }
    }
    
    void shoot()
    {
        if(currentAmmo > 0)
        {
            currentAmmo -= 1;
            if (isProjectile == false)
            {
                RaycastHit hit;
                Vector3 spread = new Vector3(bulletOrigin.transform.forward.x, bulletOrigin.transform.forward.y, bulletOrigin.transform.forward.z);
                spread = Quaternion.Euler(Random.Range(-spreadModifier1, spreadModifier1), Random.Range(-spreadModifier1, spreadModifier1), Random.Range(-spreadModifier1, spreadModifier1)) * spread;
                if (Physics.Raycast(bulletOrigin.transform.position, spread, out hit, range))
                {
                    Instantiate(coob, hit.point, Quaternion.Euler(hit.normal.x, hit.normal.y, hit.normal.z));
                    Debug.DrawLine(hit.point, bulletOrigin.transform.position, Color.black, 10f);
                }
            }
            else
            {
                towardsTarget = bulletOrigin.rotation;//Quaternion.Euler(bulletOrigin.transform.rotation.x, bulletOrigin.transform.rotation.y, bulletOrigin.transform.rotation.z);
                GameObject boolet = Instantiate(coob, new Vector3(bulletOrigin.transform.position.x, bulletOrigin.transform.position.y, bulletOrigin.transform.position.z), towardsTarget);
                //boolet.GetComponent<Rigidbody>().velocity = bulletOrigin.transform.forward * bulletSpeed;
            }
            doRecoil(recoilPos);
        }
        else{
            isReloading = true;
        }
    }
    
    void doRecoil(int position)
    {
        if(position <= recoilArrayX.Length - 1 && position <= recoilArrayY.Length - 1)
        {
            mouseMove.rotateMouseVertical(recoilArrayY[position]);
            xRecoil = recoilArrayX[position];
            characterMove.rotateHorizontally(xRecoil);
            totalXMove += xRecoil;
        }
        else{
            Debug.Log("error: recoil is outside of array size.");
            mouseMove.rotateMouseVertical(1);
            characterMove.rotateHorizontally(1);
            totalXMove += 1;
        }
    }
    
    void resetShot()
    {
        isFiring = false;
    }
}
