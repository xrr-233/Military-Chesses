using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameObject canvas;
    private MusicPlayer musicPlayer;
    private SoundPlayer soundPlayer;
    private bool redSoundPlayed, blueSoundPlayed, warSoundPlayed, victorySoundPlayed;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        /*if (musicPlayer == null)
            try
            {
                musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
            }
            catch
            {
                musicPlayer = new GameObject("MusicPlayer").AddComponent<MusicPlayer>();
            }*/
        if (soundPlayer == null)
            try
            {
                soundPlayer = GameObject.Find("SoundPlayer").GetComponent<SoundPlayer>();
            }
            catch
            {
                soundPlayer = new GameObject("SoundPlayer").AddComponent<SoundPlayer>();
            }
        redSoundPlayed = false;
        blueSoundPlayed = false;
        warSoundPlayed = false;
        victorySoundPlayed = false;


        GameObject warSystem = null;
        foreach (Transform t in canvas.transform)
        {
            if (t.name == "Deploy Red")
            {
                int i = 0;
                foreach (Transform b in t.Find("Panel").Find("Buttons"))
                {
                    int temp = i;
                    b.gameObject.GetComponent<Button>().onClick.AddListener(() => t.gameObject.GetComponent<DeployRed>().Deploy(temp));
                    i++;
                }
            }
            else if (t.name == "Deploy Blue")
            {
                int i = 0;
                foreach (Transform b in t.Find("Panel").Find("Buttons"))
                {
                    int temp = i;
                    b.gameObject.GetComponent<Button>().onClick.AddListener(() => t.gameObject.GetComponent<DeployBlue>().Deploy(temp));
                    i++;
                }
            }
            else if (t.name == "War UI")
            {
                warSystem = t.gameObject;
            }
            else if (t.name == "Surrender UI")
            {
                t.Find("Panel").Find("Button").GetComponent<Button>().onClick.AddListener(() => warSystem.GetComponent<WarSystem>().Capitulate());
            }
            else if (t.name == "Game Result")
            {
                t.gameObject.GetComponent<GameOver>().Init();
            }
        }
    }

    void Update()
    {
        foreach (Transform t in canvas.transform)
        {
            if (!t.gameObject.activeSelf)
                continue;
            if (t.name == "Deploy Red Start" && !redSoundPlayed)
            {
                soundPlayer.CustomSound(Resources.Load<AudioClip>("Sounds/Misc_Attention"));
                redSoundPlayed = true;
            }
            else if (t.name == "Deploy Red")
            {
                DeployRed.Open();
                TestArea.Open();
            }
            else if (t.name == "Deploy Blue Start" && !blueSoundPlayed)
            {
                DeployRed.Close();
                TestArea.Close();
                soundPlayer.CustomSound(Resources.Load<AudioClip>("Sounds/Misc_Attention"));
                blueSoundPlayed = true;
            }
            else if (t.name == "Deploy Blue")
            {
                DeployBlue.Open();
                TestArea.Open();
            }
            else if (t.name == "War Start" && !warSoundPlayed)
            {
                DeployBlue.Close();
                TestArea.Close();
                musicPlayer.ChangeToWar();
                soundPlayer.CustomSound(Resources.Load<AudioClip>("Sounds/DeclarationofWar"));
                warSoundPlayed = true;
            }
            else if (t.name == "War UI")
            {
                TestArea.Open();
            }
            else if (t.name == "Game Result" && !victorySoundPlayed)
            {
                TestArea.Close();
                musicPlayer.EndWar();
                victorySoundPlayed = true;
            }
        }
    }
}
