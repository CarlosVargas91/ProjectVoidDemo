using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] private GameObject bossToActivate;
    [SerializeField] private string bossRef;
    [SerializeField] private GameObject platform;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PlayerPrefs.GetInt(bossRef) < 1)
            {
                bossToActivate.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                if (platform != null)
                {
                    platform.SetActive(true);
                    platform.transform.SetParent(null);
                }
            }
        }
    }
}
