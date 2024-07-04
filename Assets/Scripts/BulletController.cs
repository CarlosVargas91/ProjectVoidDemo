using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed;
    [SerializeField] public Vector2 moveDir;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private int damageAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
        else if (collision.tag == "Boss")
        {
            BossHealthController.instance.TakeDamage(damageAmount);
        }

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        AudioManager.instance.PlaySfxAdjusted(3);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
