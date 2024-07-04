using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed= 2f;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] public GameObject fullScreenMap;

    private bool fadingToBlack;
    private bool fadingFromBlack;
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

    private void Start()
    {
        //UpdateHealth(PlayerHealthController.instance.currentHealth, PlayerHealthController.instance.maxHealth);
    }
    private void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
                fadingToBlack = false;
        }
        else if (fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
                fadingFromBlack = false;
        }

        if(Input.GetButtonDown("Pause"))
        //if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPauseGame();
        }
    }
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        fadingFromBlack = true;
        fadingToBlack = false;
    }

    public void PauseUnPauseGame()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GoToMainMenu()
    {
        Destroy(PlayerHealthController.instance.gameObject);
        PlayerHealthController.instance = null;

        Destroy(RespawnController.instance.gameObject);
        RespawnController.instance = null;

        Destroy(MapController.instance.gameObject);
        MapController.instance = null;

        instance = null;
        Destroy(gameObject);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
