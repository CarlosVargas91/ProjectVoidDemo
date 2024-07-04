using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    private enum AbilityType
    {
        DoubleJump,
        Dash,
        Ball,
        Bomb
    }

    [SerializeField] private AbilityType abilityType;
    [SerializeField] private GameObject pickupFx;
    [SerializeField] private TMP_Text unlockText;

    private string unlockMessage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerAbilityHandler player = collision.GetComponentInParent<PlayerAbilityHandler>();

            switch (abilityType) 
            {
                case AbilityType.DoubleJump:
                    player.canDoubleJump = true;
                    PlayerPrefs.SetInt("DoubleJumpUnlocked", 1);
                    unlockMessage = "Double Jump Unlocked";
                    break;
                case AbilityType.Dash:
                    player.canDash = true;
                    PlayerPrefs.SetInt("DashUnlocked", 1);
                    unlockMessage = "Dash Unlocked";
                    break;
                case AbilityType.Ball:
                    player.canBecomeBall = true;
                    PlayerPrefs.SetInt("BallUnlocked", 1);
                    unlockMessage = "Morphin Ball Unlocked";
                    break;
                case AbilityType.Bomb:
                    player.canDropBomb = true;
                    PlayerPrefs.SetInt("BombUnlocked", 1);
                    unlockMessage = "Drop Bomb Unlocked";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Instantiate(pickupFx, transform.position, Quaternion.identity);
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;

            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);

            Destroy(gameObject);
            Destroy(unlockText.transform.parent.gameObject, 3f); // Destroy Canvas

            AudioManager.instance.PlaySfx(5);
        }

    }
}
