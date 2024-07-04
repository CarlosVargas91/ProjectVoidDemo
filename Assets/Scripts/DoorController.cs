using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    private PlayerController player;
    private bool playerExitingDoor;
    private bool isShotOpen;
    private CapsuleCollider2D doorCollider;

    [SerializeField] private float distanceToOpen;
    [SerializeField] private float movePlayerSpeed;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private string levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = PlayerHealthController.instance.GetComponent<PlayerController>();
        doorCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToOpen)
        {
            OpenDoor();
        }
        else
        {
            if (isShotOpen)
                StartCoroutine(ShotCloseDoor());
            else
                CloseDoor();
        }

        if (playerExitingDoor)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, exitPoint.transform.position, movePlayerSpeed * Time.deltaTime);
        }
    }

    private void CloseDoor()
    {
        anim.SetBool("doorOpen", false);
        doorCollider.enabled = true;
    }

    public void OpenDoor()
    {
        anim.SetBool("doorOpen", true);
        doorCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            if (!playerExitingDoor)
            {
                player.canMove = false;
                StartCoroutine(UseDoorCo());
            }
        }
        else if(collision.tag == "Shot")
        {
            isShotOpen = true;
            OpenDoor();
        }
    }

    IEnumerator UseDoorCo()
    {
        playerExitingDoor = true;
        player.anim.enabled = false;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawn(exitPoint.position);
        player.canMove = true;
        player.anim.enabled = true;

        UIController.instance.StartFadeFromBlack();

        SavingGame();

        SceneManager.LoadScene(levelToLoad);
    }

    private void SavingGame()
    {
        PlayerPrefs.SetString("ContinueLevel", levelToLoad);
        PlayerPrefs.SetFloat("PosX", exitPoint.position.x);
        PlayerPrefs.SetFloat("PosY", exitPoint.position.y);
        PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);
    }

    IEnumerator ShotCloseDoor()
    {
        yield return new WaitForSeconds(4f);
        CloseDoor();
        isShotOpen = false;
    }
}
