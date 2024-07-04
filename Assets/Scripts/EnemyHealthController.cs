using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int totalHealth = 3;

    [SerializeField] private GameObject deadEffect;

    public void DamageEnemy(int _damageAmount)
    {
        totalHealth -= _damageAmount;

        if (totalHealth <= 0)
        {
            if (deadEffect != null)
                Instantiate(deadEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
            AudioManager.instance.PlaySfx(4);
        }
    }
}
