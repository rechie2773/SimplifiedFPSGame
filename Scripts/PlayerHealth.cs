using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //player health base
    public float maxHealth = 200f; 
    public float health;

    private float regenRate = 5f; // Health regeneration rate per second
    private float regenDelay = 10.0f; // Delay before continuous health regeneration starts
    private float regenTimer; // Timer for health regeneration delay
    private bool isRegenerating = false; //track if regeneration is active

    //audio
    public AudioSource audioSource; 
    public AudioClip damageSound;
    // Damage sequence tracking variables
    private bool isTakingDamage = false; // Flag to track if player is taking damage
    private float damageSoundDuration = 0.5f; // Duration of damage sound in seconds
    private float damageSoundTimer = 0; // Timer for damage sound playback

    public void Start()
    {
        health = maxHealth;        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        // Reset regeneration timer and flag
        regenTimer = regenDelay;
        isRegenerating = false;

        // check if player died
        if (health <= 0)
        {
            Die(); 
        }
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            damageSoundTimer = damageSoundDuration;
            if (audioSource && damageSound)
            {
                audioSource.PlayOneShot(damageSound);
            }
        }
    }
    private void Update()
    {
        // Handle regeneration delay and start regeneration
        if (!isRegenerating)
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                isRegenerating = true;
            }
        }

        // Regenerate health continuously if active
        if (isRegenerating && health < maxHealth)
        {
            health = Mathf.Clamp(health + (regenRate * Time.deltaTime), 0, maxHealth); // Regenerate health based on regenRate
        }
        if (isTakingDamage)
        {
            damageSoundTimer -= Time.deltaTime;
            if (damageSoundTimer <= 0)
            {
                isTakingDamage = false;
                if (audioSource && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }
    void Die()
    {
        Debug.Log("Player died");
        //change screen when player dies
        if (SceneManager.GetActiveScene().name == "FPSFog")
        {
            SceneManager.LoadScene("GameOverFog");
        }        
        else
        {
            Debug.LogWarning("Unknown scene name: " + SceneManager.GetActiveScene().name);
        }
    }
    }
