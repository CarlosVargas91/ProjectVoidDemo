using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] Transform[] patrolPoints;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float waitAtPoints;
    [SerializeField] private float jumpForce;

    private int currentPoint;
    private float waitCounter;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        waitCounter = waitAtPoints;

        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y < patrolPoints[currentPoint].position.y && rb.velocity.y < .1f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoints;
                currentPoint++;

                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
}
