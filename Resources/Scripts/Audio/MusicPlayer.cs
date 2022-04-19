using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource music;
    private AudioClip[] peaceSongList, warSongList;
    private bool[] peaceSongPlayed, warSongPlayed;
    private int peaceSongSize, warSongSize;
    private int nowPlaying;
    private int mode; // 战争状态
    private bool warIsEnd;

    // Start is called before the first frame update
    void Start()
    {
        music = gameObject.AddComponent<AudioSource>();
        warIsEnd = false;

        LoadPeaceSong();
        LoadWarSong();

        music.playOnAwake = true;
        music.clip = Resources.Load<AudioClip>("Music/hoi4 main theme allies");
        music.Play();

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!music.isPlaying && !warIsEnd)
        {
            int rand;
            if(mode == 0)
            {
                if (peaceSongSize > 0)
                {
                    do
                        rand = Random.Range(0, peaceSongList.Length);
                    while (peaceSongPlayed[rand]);

                    music.clip = peaceSongList[rand];
                    peaceSongPlayed[rand] = true;
                    peaceSongSize--;
                    nowPlaying = rand;
                    music.Play();
                }
                else
                {
                    for (int i = 0; i < peaceSongList.Length; i++)
                        if (i != nowPlaying)
                            peaceSongPlayed[i] = false;
                    do
                        rand = Random.Range(0, peaceSongList.Length);
                    while (peaceSongPlayed[rand]);

                    music.clip = peaceSongList[rand];
                    peaceSongPlayed[rand] = true;
                    peaceSongSize--;
                    nowPlaying = rand;
                    music.Play();
                }
            }
            else
            {
                if (warSongSize > 0)
                {
                    do
                        rand = Random.Range(0, warSongList.Length);
                    while (warSongPlayed[rand]);

                    music.clip = warSongList[rand];
                    warSongPlayed[rand] = true;
                    warSongSize--;
                    nowPlaying = rand;
                    music.Play();
                }
                else
                {
                    for (int i = 0; i < warSongList.Length; i++)
                        if (i != nowPlaying)
                            warSongPlayed[i] = false;
                    do
                        rand = Random.Range(0, warSongList.Length);
                    while (warSongPlayed[rand]);

                    music.clip = warSongList[rand];
                    warSongPlayed[rand] = true;
                    warSongSize--;
                    nowPlaying = rand;
                    music.Play();
                }
            }
        }
    }

    public void LoadPeaceSong()
    {
        string peacePath = Application.dataPath + "/Resources/Music/peace";

        DirectoryInfo folder = new DirectoryInfo(peacePath);
        FileInfo[] peaceSongFile = folder.GetFiles("*.mp3");
        peaceSongSize = peaceSongFile.Length;
        peaceSongList = new AudioClip[peaceSongSize];
        peaceSongPlayed = new bool[peaceSongSize];

        for (int i = 0; i < peaceSongSize; i++)
        {
            string temp = Path.GetFileNameWithoutExtension(peaceSongFile[i].Name);
            peaceSongList[i] = Resources.Load<AudioClip>("Music/peace/" + temp);
        }
    }

    public void LoadWarSong()
    {
        string warPath = Application.dataPath + "/Resources/Music/war_play_1";
        DirectoryInfo folder = new DirectoryInfo(warPath);
        FileInfo[] warSongFile = folder.GetFiles("*.mp3");
        warSongSize = warSongFile.Length;
        warSongList = new AudioClip[warSongSize];
        warSongPlayed = new bool[warSongSize];

        for (int i = 0; i < warSongSize; i++)
        {
            string temp = Path.GetFileNameWithoutExtension(warSongFile[i].Name);
            warSongList[i] = Resources.Load<AudioClip>("Music/war_play_1/" + temp);
        }
    }

    public void ChangeToWar()
    {
        string[] basicSongs = { "周璇 - 凯旋歌", "Geoff Knorr - Oda Nobunaga War - Japan - Rokudan no Shirabe", "亮剑(《亮剑》电视主题曲)", "aggression" };
        int rand = Random.Range(0, basicSongs.Length);
        string basicSong = basicSongs[rand];

        for (int i = 0; i < warSongSize; i++)
        {
            print(basicSong);
            if (warSongList[i].name.Equals(basicSong))
            {
                music.clip = warSongList[i];
                warSongPlayed[i] = true;
                nowPlaying = i;
                break;
            }
        }

        mode = 1;
        warSongSize--;
        music.Play();
    }

    public void EndWar()
    {
        warIsEnd = true;
        music.clip = Resources.Load<AudioClip>("Music/soviet victory");
        music.Play();
    }
}
