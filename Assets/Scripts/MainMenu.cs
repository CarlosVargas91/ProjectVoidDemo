using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private PlayerAbilityHandler player;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButton.SetActive(true);
        }
        AudioManager.instance.PlayMainMenuMusic();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("Area1");
    }

    public void ContinueGame()
    {
        player.gameObject.SetActive(true);
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        player.canDoubleJump = PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1;
        player.canDash = PlayerPrefs.GetInt("DashUnlocked") == 1;
        player.canBecomeBall = PlayerPrefs.GetInt("BallUnlocked") == 1;
        player.canDropBomb = PlayerPrefs.GetInt("BombUnlocked") == 1;

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
