using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
*/

/// <summary>
/// 音效
/// </summary>
public class SoundPlayer : MonoBehaviour
{
    private AudioSource sound;
    public AudioClip addSound;


    // Start is called before the first frame update
    private void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        sound.playOnAwake = false;
        sound.clip = addSound;
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.name.Contains("Canvas") && gameObject.transform.GetComponent<Canvas>().enabled)
        {
            sound.Play();
            this.enabled = false;
        }
    }

    public void ButtonClicked()
    {
        sound.Play();
    }
}
