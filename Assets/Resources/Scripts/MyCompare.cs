using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MyCompare : IComparer<GameObject>
{
    public MyCompare()
    {
    }

    //�ȽϷ���
    public int Compare(GameObject x, GameObject y)
    {
        return y.name.CompareTo(x.name);
    }
}