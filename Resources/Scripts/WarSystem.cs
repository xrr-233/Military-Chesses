using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
*/

/// <summary>
/// 
/// </summary>
public class WarSystem : MonoBehaviour
{
    private static int round;
    private static int blueManpower, redManpower;
    private static bool isRedRound;

    private static Color yellow = new Color(255, 255, 0), black = new Color(0, 0, 0);

    private static GameObject Round, RedManpower, BlueManpower, RedBool, BlueBool;

    // Start is called before the first frame update
    private void Start()
    {
        round = 2;
        blueManpower = 25;
        redManpower = 25;

        Round = transform.GetChild(0).Find("Round").gameObject;
        Round.transform.GetComponent<Text>().text = (round / 2).ToString();

        RedManpower = transform.GetChild(0).Find("RedManpower").gameObject;
        RedManpower.transform.GetComponent<Text>().text = redManpower.ToString();
        BlueManpower = transform.GetChild(0).Find("BlueManpower").gameObject;
        BlueManpower.transform.GetComponent<Text>().text = blueManpower.ToString();

        isRedRound = true;
        RedBool = transform.GetChild(0).Find("RedBool").gameObject;
        RedBool.transform.GetComponent<Text>().color = yellow;
        BlueBool = transform.GetChild(0).Find("BlueBool").gameObject;
        BlueBool.transform.GetComponent<Text>().color = black;
    }

    public static void ChangeSide()
    {
        isRedRound ^= true;
        round++;
        Round.transform.GetComponent<Text>().text = (round / 2).ToString();
        RedBool.GetComponent<Text>().color = isRedRound? yellow : black;
        BlueBool.GetComponent<Text>().color = isRedRound ? black : yellow;
    }

    public static void RedReduce()
    {
        redManpower--;
        RedManpower.transform.GetComponent<Text>().text = redManpower.ToString();
    }
    public static void BlueReduce()
    {
        blueManpower--;
        BlueManpower.transform.GetComponent<Text>().text = blueManpower.ToString();
    }

    public void Capitulate()
    {
        if(isRedRound)
            GameOver.Win(false);
        else
            GameOver.Win(true);
    }
}
