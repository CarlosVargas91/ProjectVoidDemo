using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private GameObject destroyEffect;
    public bool destroyOnDamage;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        PlayerHealthController.instance.DamagePlayer(damageAmount);

        if (destroyOnDamage)
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
