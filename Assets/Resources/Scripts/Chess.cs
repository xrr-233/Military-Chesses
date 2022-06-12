using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
*/

/// <summary>
/// 
/// </summary>
public class Chess : MonoBehaviour
{
    private string side;
    private int level;  

    private void Awake()
    {
        Vector3 temp = new Vector3(0, (float)0.05, 0);
        this.transform.position = temp;
        temp = new Vector3(90, 0, 180);
        this.transform.eulerAngles = temp;
        temp = new Vector3((float)0.05, (float)0.05, (float)0.05);
        this.transform.localScale = temp;
        Renderer[] temp2 = gameObject.GetComponentsInChildren<Renderer>();
        temp2[0].enabled = false;
        temp2[1].enabled = false;

        this.side = this.gameObject.transform.parent.name;
        this.level = GetLevel(this.gameObject);

    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SetPosition(Vector3 position)
    {
        Vector3 temp = new Vector3(position.x, (float)(position.y + 0.05), position.z);
        this.transform.position = temp;
        Renderer[] temp2 = gameObject.GetComponentsInChildren<Renderer>();
        temp2[0].enabled = true;
        temp2[1].enabled = true;
    }

    private void Dead()
    {
        Vector3 temp = new Vector3(0, (float)0.05, 0);
        this.transform.position = temp;
        Renderer[] temp2 = gameObject.GetComponentsInChildren<Renderer>();
        temp2[0].enabled = false;
        temp2[1].enabled = false;
    }

    public string GetSide()
    {
        return this.side;
    }

    public int GetLevel()
    {
        return this.level;
    }

    public static int GetLevel(GameObject g)
    {
        char[] temp = g.name.ToCharArray();
        if (temp[0] != 'i')
            return temp[0] - '0';
        else
        {
            if (temp[1] == 'i' && temp[2] == 'i')
                return -3;
            else if (temp[1] == 'i')
                return -2;
            else return -1;
        }
    }
}
