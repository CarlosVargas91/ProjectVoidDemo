using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    [SerializeField] AudioManager aM;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager newAm = Instantiate(aM);
            AudioManager.instance = newAm;
            DontDestroyOnLoad(newAm.gameObject);
        }
    }
}
