using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
*/

/// <summary>
/// 
/// </summary>
public class GameOver : MonoBehaviour
{
    private static bool isRedWin;
    private static Texture Japan, China;
    private static GameObject I;

    public void Init()
    {
        isRedWin = false;
        Japan = Resources.Load<Texture>("Textures/Japan");
        China = Resources.Load<Texture>("Textures/China");

        I = gameObject;
    }

    public static void Win(bool b)
    {
        isRedWin = b;
        if (isRedWin)
        {
            I.transform.GetChild(0).GetComponent<RawImage>().texture = Japan;
            I.transform.GetChild(1).GetComponent<Text>().text = "红方胜利！";
            I.transform.GetChild(1).GetComponent<Text>().color = new Color(255, 0, 0);
        }
        else
        {
            I.transform.GetChild(0).GetComponent<RawImage>().texture = China;
            I.transform.GetChild(1).GetComponent<Text>().text = "蓝方胜利！";
            I.transform.GetChild(1).GetComponent<Text>().color = new Color(0, 0, 255);
        }
    }
}
