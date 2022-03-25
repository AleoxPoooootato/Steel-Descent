using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 2000;
    public int maxShields = 1000;
    public int currentHealth;
    public int currentShields;
    public int shieldRegenSpeed = 100;
    public int shieldRegenDelay = 3;
    public int healthRegenSpeed = 100;
    public float healthRegenDelay = 0.5f;
    public bool inCombat;
    private Coroutine shieldRegenerationActive;
    private Coroutine healthRegenerationActive;
    
    void Start()
    {
        currentHealth = maxHealth;
        currentShields = maxShields;
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(200);
        }
    }*/

    public void healShields(int healAmount)
    {
        currentShields += healAmount;
        Debug.Log("Shields healed for " + healAmount);
    }

    public void healHealth(int healAmount)
    {
        currentHealth += healAmount;
        Debug.Log("Health healed for " + healAmount);
    }

    public void startHealthRegen()
    {
        if (healthRegenerationActive == null)
        {
            healthRegenerationActive = StartCoroutine(regenHealth());
        }
    }

    public void stopHealthRegen()
    {
        StopCoroutine(healthRegenerationActive);
    }

    public void takeDamage(int damage)
    {
        int damageRollover = 0;
        currentShields -= damage;
        Debug.Log("Shields took " + damage + " damage");
        if (currentShields < 0)
        {
            damageRollover = Mathf.Abs(currentShields);
            Debug.Log("Damage rollover is " + damageRollover + " damage");
            currentShields = 0;
        }
        currentHealth -= damageRollover;
        Debug.Log("Health took " + damageRollover + " damage");
        if (currentHealth < 0)
        {
            Debug.Log("I'm dead");
            currentHealth = 0;
        }

        if (shieldRegenerationActive != null)
        { 
            StopCoroutine(shieldRegenerationActive);
        }
        if (healthRegenerationActive != null)
        { 
            StopCoroutine(healthRegenerationActive);
        }

        shieldRegenerationActive = StartCoroutine(regenShields());
    }
    
    private IEnumerator regenShields()
    {
        yield return new WaitForSeconds(shieldRegenDelay);
        while (currentShields < maxShields)
        {
            healShields(1);
            if (currentShields > maxShields)
            {
                currentShields = maxShields;
                Debug.Log("Regen Stopped. Shields are " + currentShields);
                yield break;
            }
            Debug.Log("Shields Regen, " + currentShields);
            yield return new WaitForSeconds(1 / shieldRegenSpeed);
        }
    }

    private IEnumerator regenHealth()
    {
        yield return new WaitForSeconds(healthRegenDelay);
        while (currentHealth < maxHealth)
        {
            healHealth(1);
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
                Debug.Log("Regen Stopped. Health is " + currentHealth);
                yield break;
            }
            Debug.Log("Health Regen, " + currentHealth);
            yield return new WaitForSeconds(1 / healthRegenSpeed);
        }
    }
}
