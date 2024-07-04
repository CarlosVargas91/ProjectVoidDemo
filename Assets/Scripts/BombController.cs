using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private PlayerController player;
    private EnemyHealthController enemy;

    [SerializeField] private float timetoExplode = .5f;
    [SerializeField] private GameObject explosion;

    [SerializeField] private float blastRange;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private LayerMask interacTableLayer;
    //[SerializeField] private LayerMask whatIsPlayer;
    //[SerializeField] private LayerMask whatIsEnemy;
    //[SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<EnemyHealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        timetoExplode -= Time.deltaTime;

        if (timetoExplode <= 0)
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);

            BombCollision();
        }
    }


    private void BombCollision()
    {
        Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, interacTableLayer);

        foreach (Collider2D col in objectsToRemove)
        {
            if(LayerMask.LayerToName(col.gameObject.layer) == "Destructible")
                Destroy(col.gameObject);

            if (LayerMask.LayerToName(col.gameObject.layer) == "Enemy")
                col.gameObject.GetComponent<EnemyHealthController>()?.DamageEnemy(damageAmount);

            if (LayerMask.LayerToName(col.gameObject.layer) == "Player")
                player.BallHitJump();

            if (LayerMask.LayerToName(col.gameObject.layer) == "Boss")
                col.gameObject.GetComponent<BossHealthController>()?.TakeDamage(damageAmount);
        }
        AudioManager.instance.PlaySfxAdjusted(4);
    }

    //private void CollideWithPlayer()
    //{
    //    Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, .5f, whatIsPlayer);

    //    if (playerCollider != null)
    //    {
    //        player.BallHitJump();
    //    }
    //}
    //private void CollideWithEnemy()
    //{
    //    Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsEnemy);

    //    foreach (Collider2D col in enemyCollider)
    //    {
    //        EnemyHealthController enemy = col.GetComponent<EnemyHealthController>();

    //        if (enemy != null)
    //        {
    //            enemy.DamageEnemy(damageAmount);
    //        }
    //    }
    //}

}
