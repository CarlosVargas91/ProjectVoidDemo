using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAbilityHandler abilities;

    [SerializeField] public Animator anim;
    [SerializeField] private Animator ballanim;

    private bool isOnGround;
    private bool canDoubleJump;
    public bool canMove;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private float dashCounter;

    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask whatisGround;

    [SerializeField] private BulletController shotPreFab;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private Transform shotPointVertical;

    [Header("Dash setup")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject afterImagePreFab;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private float timeBetweenAfterImages;
    [SerializeField] private float dashCoolDown;
    private float dashRechargeCounter;
    [SerializeField] private Color afterImageColor;
    private float afterImageCounter;

    [Header("Ball setup")]
    [SerializeField] private GameObject standing;
    [SerializeField] private GameObject ball;
    [SerializeField] private float waitToball;
    private float ballCounter;

    [Header("Bomb setup")]
    [SerializeField] private Transform bombPoint;
    [SerializeField] private GameObject bomb;

    private bool canShoot = true;
    private bool canWalk = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityHandler>();
        //anim = GetComponentsInChildren<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Time.timeScale != 0)
        {

            MovementAndDash();

            ShootAndBallDrop();

            BallMode();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("upShoot", Input.GetAxisRaw("Vertical"));

            if (Input.GetAxisRaw("Vertical") > 0.1f)
            {
                if (isOnGround)
                    rb.velocity = new Vector2(0f, rb.velocity.y);

                canWalk = false;
            }
            else
                canWalk = true;

        }

        if (ball.activeSelf)
        {
            ballanim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
    }

    private void BallMode()
    {
        if (!ball.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -.9f && abilities.canBecomeBall)
            {
                ballCounter -= Time.deltaTime;

                if (ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                    AudioManager.instance.PlaySfx(6);
                }
            }
            else
            {
                ballCounter = waitToball;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > .9f)
            {
                ballCounter -= Time.deltaTime;

                if (ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                    AudioManager.instance.PlaySfx(10);
                }
            }
            else
            {
                ballCounter = waitToball;
            }
        }
    }

    private void ShootAndBallDrop()
    {
        if (Input.GetButtonDown("Fire1") && (canShoot || isOnGround))
        {
            if (standing.activeSelf)
            {
                if (Input.GetAxisRaw("Vertical") > 0f)
                {
                    VerticalShot();
                }
                else
                {
                    HorizontalShot();
                }
            }
            else if (ball.activeSelf && abilities.canDropBomb)
            {
                Instantiate(bomb, bombPoint.position, Quaternion.identity);
                AudioManager.instance.PlaySfxAdjusted(13);
            }
        }
    }

    private void HorizontalShot()
    {
        Instantiate(shotPreFab, shotPoint.position, Quaternion.identity).moveDir = new Vector2(transform.localScale.x, 0f);
        anim.SetTrigger("shotFired");
        AudioManager.instance.PlaySfxAdjusted(14);
    }

    private void VerticalShot()
    {
        Instantiate(shotPreFab, shotPointVertical.position, Quaternion.identity).moveDir = new Vector2(0f, transform.localScale.y);
        anim.SetTrigger("shotFired");
        AudioManager.instance.PlaySfxAdjusted(14);
    }

    private void MovementAndDash()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Dash") && standing.activeSelf && abilities.canDash)
            {
                dashCounter = dashTime;
                ShowAfterImage();
                AudioManager.instance.PlaySfxAdjusted(7);
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);
            afterImageCounter -= Time.deltaTime;

            if (afterImageCounter <= 0)
                ShowAfterImage();

            dashRechargeCounter = dashCoolDown;
        }
        else
        {
            if (canWalk)
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

            FlipCharacter();
        }

        Jump();
    }

    private void Jump()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatisGround);

        if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
        {
            if (isOnGround)
            {
                canDoubleJump = true;
                canShoot = true;
                AudioManager.instance.PlaySfxAdjusted(12);
            }
            else
            {
                canDoubleJump = false;
                canShoot = false;
                anim.SetTrigger("doubleJump");
                AudioManager.instance.PlaySfxAdjusted(9);
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void FlipCharacter()
    {
        if (rb.velocity.x < 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (rb.velocity.x > 0.1f)
            transform.localScale = Vector3.one;

        //Debug.Log("The velocity is " + rb.velocity.x);
    }

    public void ShowAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePreFab, transform.position, Quaternion.identity);
        SpriteRenderer image = afterImage.GetComponent<SpriteRenderer>();

        if (image != null)
        {
            image.sprite = sr.sprite;
            image.transform.localScale = transform.localScale;
            image.color = afterImageColor;
        }

        Destroy(afterImage, afterImageLifeTime);

        afterImageCounter = timeBetweenAfterImages;
    }

    public void BallHitJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 15f);
    }
}
