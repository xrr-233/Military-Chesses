using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MyCompare : IComparer<GameObject>
{
    public MyCompare()
    {
    }

    //比较方法
    public int Compare(GameObject x, GameObject y)
    {
        return y.name.CompareTo(x.name);
    }
}