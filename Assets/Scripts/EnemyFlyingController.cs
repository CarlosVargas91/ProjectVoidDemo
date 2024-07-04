using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    [SerializeField] private float rangeToChase;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    private bool isChasing;
    private Transform player;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.transform;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position, player.position) < rangeToChase)
            {
                isChasing = true;
                anim.SetBool("isChasing", isChasing);
            }
        }
        else
        {
            if (player.gameObject.activeSelf)
            {
                RotateToPlayer();

                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        transform.position += -transform.right * moveSpeed * Time.deltaTime;
    }

    private void RotateToPlayer()
    {
        Vector3 direction = transform.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
    }
}
