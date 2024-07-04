using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource mainMenuMusic, levelMusic, bossMusic;
    [SerializeField] private AudioSource[] sfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic.isPlaying)
            return;

        levelMusic.Stop();
        bossMusic.Stop();

        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {
        if (levelMusic.isPlaying)
            return;

        mainMenuMusic.Stop();
        bossMusic.Stop();

        levelMusic.Play();
    }

    public void PlayBossMusic()
    {
        if (bossMusic.isPlaying)
            return;

        levelMusic.Stop();
        mainMenuMusic.Stop();

        bossMusic.Play();
    }

    public void PlaySfx(int sfxIndex)
    {
        sfx[sfxIndex].Stop();
        sfx[sfxIndex].Play();
    }

    public void PlaySfxAdjusted(int sfxIndex)
    {
        sfx[sfxIndex].pitch = Random.Range(.8f, 1.2f);
        PlaySfx(sfxIndex);
    }
}
