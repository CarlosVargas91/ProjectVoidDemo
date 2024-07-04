using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [HideInInspector] public int currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] private float invincibilityLength;
    [SerializeField] private float flashLength;
    [SerializeField] private SpriteRenderer[] playerSprites;

    private float invincinCounter;
    private float flashCounter;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (invincinCounter > 0)
        {
            invincinCounter -= Time.deltaTime;

            FlashPlayer();
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //Do not destroy the player
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FlashPlayer()
    {
        flashCounter -= Time.deltaTime;
        if (flashCounter <= 0)
        {
            foreach (SpriteRenderer sr in playerSprites)
            {
                sr.enabled = !sr.enabled;
            }
            flashCounter = flashLength;
        }

        if (invincinCounter <= 0)
        {
            foreach (SpriteRenderer sr in playerSprites)
            {
                sr.enabled = true;
            }
            flashCounter = 0f;
        }
    }

    public void DamagePlayer(int _amount)
    {
        if (invincinCounter <= 0)
        {
            currentHealth -= _amount;

            if (currentHealth <= 0)
            {
                PlayerDead();
            }
            else
            {
                invincinCounter = invincibilityLength;
                AudioManager.instance.PlaySfxAdjusted(11);
            }
            UpdateUIHealth();
        }
    }

    public void PlayerDead()
    {
        currentHealth = 0;
        RespawnController.instance.Respawn();

        AudioManager.instance.PlaySfx(8);
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;

        UpdateUIHealth();
    }

    public void HealPlayer(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateUIHealth();
    }

    private void UpdateUIHealth()
    {
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }
}
