using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public static BossHealthController instance;
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] public int currentHealth = 30;
    private BossBattle theBoss;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        theBoss = GetComponent<BossBattle>();
        bossHealthSlider.maxValue = currentHealth;
        bossHealthSlider.value = currentHealth;
    }
    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            theBoss.EndBattle();
            AudioManager.instance.PlaySfx(0);
            //Destroy(gameObject);
        }
        else
        {
            AudioManager.instance.PlaySfx(1);
        }

        bossHealthSlider.value = currentHealth;
    }
}
