using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource sound;
    private AudioClip UIButtonSound;
    private AudioClip sd_clicked;
    private AudioClip sd_withdraw;
    private AudioClip[] sd_deploy = new AudioClip[3];
    private AudioClip[] sd_change = new AudioClip[3];
    private AudioClip sd_move;
    private AudioClip sd_win;
    private AudioClip sd_draw;
    private AudioClip sd_lose;

    private Button[] buttons;

    private void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioListener>();
        sound.playOnAwake = false;
        UIButtonSound = Resources.Load<AudioClip>("Sounds/pause");
        sd_clicked = Resources.Load<AudioClip>("Sounds/sfx_select");
        sd_withdraw = Resources.Load<AudioClip>("Sounds/sfx_click");
        sd_deploy[0] = Resources.Load<AudioClip>("Sounds/select_army_01");
        sd_deploy[1] = Resources.Load<AudioClip>("Sounds/select_army_02");
        sd_deploy[2] = Resources.Load<AudioClip>("Sounds/select_army_03");
        sd_change[0] = Resources.Load<AudioClip>("Sounds/select_army_04");
        sd_change[1] = Resources.Load<AudioClip>("Sounds/select_army_04");
        sd_change[2] = Resources.Load<AudioClip>("Sounds/select_army_04");
        sd_move = Resources.Load<AudioClip>("Sounds/GI_InfantryMove");
        sd_win = Resources.Load<AudioClip>("Sounds/sfx_occupy");
        sd_draw = Resources.Load<AudioClip>("Sounds/sfx_draw");
        sd_lose = Resources.Load<AudioClip>("Sounds/sfx_dengdengdong");

        DontDestroyOnLoad(gameObject);
    }

    public void UIButtonClicked()
    {
        sound.clip = UIButtonSound;
        sound.Play();
    }

    public void Click()
    {
        sound.clip = sd_clicked;
        sound.Play();
    }

    public void Withdraw()
    {
        sound.clip = sd_withdraw;
        sound.Play();
    }

    public void Deploy()
    {
        sound.clip = sd_deploy[Random.Range(0, 3)];
        sound.Play();
    }

    public void Change()
    {
        sound.clip = sd_change[Random.Range(0, 3)];
        sound.Play();
    }

    public void Move()
    {
        sound.clip = sd_move;
        sound.Play();
    }

    public void Win()
    {
        sound.clip = sd_win;
        sound.Play();
    }

    public void Draw()
    {
        sound.clip = sd_draw;
        sound.Play();
    }

    public void Lose()
    {
        sound.clip = sd_lose;
        sound.Play();
    }

    public void CustomSound(AudioClip addSound)
    {
        sound.clip = addSound;
        sound.Play();
    }

    private void Update()
    {
        buttons = FindObjectsOfType(typeof(Button)) as Button[];
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => UIButtonClicked());
        }
    }
}
