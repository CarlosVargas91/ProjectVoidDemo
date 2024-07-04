using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController cam;
    private Animator anim;
    [SerializeField] private Transform camPosition;
    [SerializeField] private Transform theBoss;
    [SerializeField] private float camSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int treshold1, treshold2;
    [SerializeField] private float activeTime, fadeOutTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;
    [SerializeField] private Transform[] spawnPoints;
    private Transform targetPoint;

    [Header("Shot battle")]
    [SerializeField] private float timeBetweenShots1, timeBetweenShots2;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shotPoint;
    private float shotCounter;

    [SerializeField] private string bossRef;

    [Header("Win")]
    [SerializeField] private GameObject winObjects;
    private bool battleEnded;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        anim = GetComponentInChildren<Animator>();
        cam.enabled = false;

        activeCounter = activeTime;
        shotCounter = timeBetweenShots1;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (!battleEnded)
        {
            if (BossHealthController.instance.currentHealth > treshold1)
            {
                Phase1();
            }
            else
            {
                Phase2();
            }
        }
        else
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter < 0)
            {
                WinObjects();

                cam.enabled = true;
                gameObject.SetActive(false);

                AudioManager.instance.PlayLevelMusic();

                PlayerPrefs.SetInt(bossRef, 1);
            }
        }
    }

    private void WinObjects()
    {
        if (winObjects != null)
        {
            winObjects.SetActive(true);
            winObjects.transform.SetParent(null);
        }
    }

    private void Phase2()
    {
        if (targetPoint == null)
        {
            targetPoint = theBoss;
            fadeCounter = fadeOutTime;
            anim.SetTrigger("vanish");
        }
        else
        {
            if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)
            {
                theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f)
                {
                    fadeCounter = fadeOutTime;
                    anim.SetTrigger("vanish");
                }

                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    if (BossHealthController.instance.currentHealth > treshold2)
                    {
                        shotCounter = timeBetweenShots1;
                    }
                    else
                    {
                        shotCounter = timeBetweenShots2;
                    }
                    Instantiate(bullet, shotPoint.position, Quaternion.identity);
                }
            }
            else if (fadeCounter > 0)
            {
                fadeCounter -= Time.deltaTime;
                if (fadeCounter <= 0)
                {
                    theBoss.gameObject.SetActive(false);
                    inactiveCounter = inactiveTime;
                }
            }
            else if (inactiveCounter > 0)
            {
                inactiveCounter -= Time.deltaTime;

                if (inactiveCounter <= 0)
                {
                    theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                    targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    int whileBreaker = 0;
                    while (targetPoint.position == theBoss.position && whileBreaker < 100)
                    {
                        targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        whileBreaker++;
                    }

                    theBoss.gameObject.SetActive(true);

                    if (BossHealthController.instance.currentHealth > treshold2)
                    {
                        shotCounter = timeBetweenShots1;
                    }
                    else
                    {
                        shotCounter = timeBetweenShots2;
                    }
                }
            }
        }
    }

    private void Phase1()
    {
        if (activeCounter > 0)
        {
            activeCounter -= Time.deltaTime;

            if (activeCounter <= 0)
            {
                fadeCounter = fadeOutTime;
                anim.SetTrigger("vanish");
            }

            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots1;
                Instantiate(bullet, shotPoint.position, Quaternion.identity);
            }
        }
        else if (fadeCounter > 0)
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter <= 0)
            {
                theBoss.gameObject.SetActive(false);
                inactiveCounter = inactiveTime;
            }
        }
        else if (inactiveCounter > 0)
        {
            inactiveCounter -= Time.deltaTime;

            if (inactiveCounter <= 0)
            {
                theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                theBoss.gameObject.SetActive(true);
                activeCounter = activeTime;
                shotCounter = timeBetweenShots1;
            }
        }
    }

    public void EndBattle()
    {
        battleEnded = true;
        fadeCounter = fadeOutTime;
        anim.SetTrigger("vanish");
        theBoss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsOfType<BossBullet>();

        if (bullets.Length > 0)
        {
            foreach (BossBullet bb in bullets)
            {
                Destroy(bb.gameObject);
            }
        }
    }
}
