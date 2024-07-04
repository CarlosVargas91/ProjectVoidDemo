using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;

    [SerializeField] float waitToRespawn;
    [SerializeField] private GameObject deathEffect;
    private Vector3 respawnPoint;
    private GameObject thePlayer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = PlayerHealthController.instance.gameObject;

        respawnPoint = thePlayer.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoRoutine());
    }

    IEnumerator RespawnCoRoutine()
    {
        thePlayer.SetActive(false);

        if (deathEffect != null)
        {
            Instantiate(deathEffect, thePlayer.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitToRespawn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        thePlayer.transform.position = respawnPoint;
        thePlayer.SetActive(true);
        PlayerHealthController.instance.RestoreHealth();
    }

    public void SetSpawn(Vector3 position)
    {
        respawnPoint = position;
    }
}
