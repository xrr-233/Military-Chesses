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
public class DeployRed : MonoBehaviour
{
    // Start is called before the first frame update
    private static List<GameObject> ProvincesAvailable;
    private static List<GameObject> ProvincesOccupied;

    private static Material mt_normal, mt_canGo, mt_canGoHovering, mt_canEat, mt_canEatHovering;

    private static int[] quant, total, ident;
    private static int choosing, count;
    private static bool[] used;

    private static GameObject chess;
    private static GameObject provinceClicked;
    private static GameObject BtnNext, BtnRemove;
    private static GameObject Number;

    private static bool isAvailable;

    private void Start()
    {
        ProvincesAvailable = new List<GameObject>();
        ProvincesOccupied = new List<GameObject>();

        quant = new int[] { 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2 };
        total = new int[] { 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 2 };
        ident = new int[11];
        ident[0] = 0;
        for (int i = 1; i < ident.Length; i++)
            ident[i] = ident[i - 1] + quant[i - 1];
        choosing = -1;
        count = 24;
        used = new bool[24];

        mt_normal = Resources.Load<Material>("Materials/mt_normal");
        mt_canGo = Resources.Load<Material>("Materials/mt_canGo");
        mt_canGoHovering = Resources.Load<Material>("Materials/mt_canGoHovering");
        mt_canEat = Resources.Load<Material>("Materials/mt_canEat");
        mt_canEatHovering = Resources.Load<Material>("Materials/mt_canEatHovering");

        GameObject Provinces = GameObject.Find("Provinces");

        ProvincesAvailable.Add(Provinces.transform.Find("Hebei").Find("Tangshan").gameObject);
        ProvincesAvailable.Add(Provinces.transform.Find("Hebei").Find("Chengde").gameObject);
        ProvincesAvailable.Add(Provinces.transform.Find("Hebei").Find("Qinhuangdao").gameObject);

        ProvincesAvailable.Add(Provinces.transform.Find("Neimenggu").Find("Chifeng").gameObject);
        ProvincesAvailable.Add(Provinces.transform.Find("Neimenggu").Find("Tongliao").gameObject);
        ProvincesAvailable.Add(Provinces.transform.Find("Neimenggu").Find("Hulunbeier").gameObject);
        ProvincesAvailable.Add(Provinces.transform.Find("Neimenggu").Find("Manzhouli").gameObject);

        foreach (Transform t in Provinces.transform.Find("Liaoning"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Jilin"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Heilongjiang"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Taiwan"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Chaoxian"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Hanguo"))
            ProvincesAvailable.Add(t.gameObject);
        foreach (Transform t in Provinces.transform.Find("Riben"))
            ProvincesAvailable.Add(t.gameObject);

        ProvincesAvailable.Remove(Provinces.transform.Find("Riben").Find("Dongjing").gameObject);

        Number = this.transform.GetChild(0).Find("Number").gameObject;
        for (int i = 0; i < Number.transform.childCount; i++)
        {
            Number.transform.GetChild(i).GetComponent<Text>().text = quant[i].ToString() + "/" + total[i].ToString();
        }

        BtnNext = this.transform.Find("Button").gameObject;
        BtnNext.SetActive(false);
        BtnRemove = this.transform.Find("Button (1)").gameObject;
        BtnRemove.SetActive(false);

        isAvailable = false;

    }

    // Update is called once per frame
    private void Update()
    {

    }

    #region 按按钮
    public void Deploy(int level)
    {
        UndoRemoveAsk();

        if (choosing == -1)//未选择
        {
            if (quant[level] <= 0)
            {
                print("用完了");
                return;
            }

            GameObject allChesses = GameObject.Find("Red");
            for(int i = ident[level]; i < ident[level] + total[level]; i++)
                if (!used[i])
                    chess = allChesses.transform.GetChild(i).gameObject;
            

            quant[level]--;

            Number.transform.GetChild(level).GetComponent<Text>().text = quant[level].ToString() + "/" + total[level].ToString();

            foreach (GameObject g in ProvincesAvailable)
                g.transform.GetComponent<MeshRenderer>().material = mt_canGo;
            foreach (GameObject g in ProvincesOccupied)
                g.transform.GetComponent<MeshRenderer>().material = mt_canEat;

            choosing = level;
        }
        else if (level == choosing)//选择了，但选择的是上次选择的
        {
            quant[level]++;

            Number.transform.GetChild(level).GetComponent<Text>().text = quant[level].ToString() + "/" + total[level].ToString();

            foreach (GameObject g in ProvincesAvailable)
                g.transform.GetComponent<MeshRenderer>().material = mt_normal;
            foreach (GameObject g in ProvincesOccupied)
                g.transform.GetComponent<MeshRenderer>().material = mt_canEat;

            choosing = -1;
        }
        else
        {

            GameObject allChesses = GameObject.Find("Red");
            for (int i = ident[level]; i < ident[level] + total[level]; i++)
                if (!used[i])
                    chess = allChesses.transform.GetChild(i).gameObject;

            quant[choosing]++;
            quant[level]--;

            Number.transform.GetChild(choosing).GetComponent<Text>().text = quant[choosing].ToString() + "/" + total[choosing].ToString();
            Number.transform.GetChild(level).GetComponent<Text>().text = quant[level].ToString() + "/" + total[level].ToString();

            choosing = level;

        }

    }
    #endregion

    #region 当前选择的职位
    public static void ClearChoosing(GameObject province, GameObject isClickedChess)
    {
        ProvincesAvailable.Remove(province);
        ProvincesOccupied.Add(province);
        choosing = -1;
        count--;

        province.transform.GetComponent<MeshRenderer>().material = mt_normal;
        foreach (GameObject g in ProvincesAvailable)
            g.transform.GetComponent<MeshRenderer>().material = mt_normal;
        foreach (GameObject g in ProvincesOccupied)
            g.transform.GetComponent<MeshRenderer>().material = mt_normal;

        used[IsChildOf(isClickedChess)] = true;

        if (count == 0)
            BtnNext.SetActive(true);
    }

    public static void UndoClearChoosing(GameObject province, GameObject isClickedChess)
    {
        ProvincesOccupied.Remove(province);
        ProvincesAvailable.Add(province);
        count++;

        int level = Chess.GetLevel(isClickedChess.gameObject);
        level--;
        if (level == -3)
            level = 10;
        if (level == -2)
            level = 9;

        quant[level]++;

        used[IsChildOf(isClickedChess)] = false;

        Number.transform.GetChild(level).GetComponent<Text>().text = quant[level].ToString() + "/" + total[level].ToString();
    }
    #endregion

    public static List<GameObject> GetList()
    {
        return ProvincesOccupied;
    }

    #region 控制组件可用性
    public static void Open()
    {
        isAvailable = true;
    }

    public static void Close()
    {
        isAvailable = false;
    }

    public static bool Status()
    {
        return isAvailable;
    }

    public void OpenForButton()
    {
        isAvailable = true;
        TestArea.Open();
    }

    public void CloseForButton()
    {
        isAvailable = false;
        TestArea.Close();
    }
    #endregion

    #region 放回
    public static void RemoveAsk(GameObject isClickedChess, GameObject province)
    {
        chess = isClickedChess;
        provinceClicked = province;
        BtnRemove.SetActive(true);
    }
    public static void UndoRemoveAsk()
    {
        chess = null;
        provinceClicked = null;
        BtnRemove.SetActive(false);
    }
    public void Remove()
    {
        provinceClicked.transform.GetComponent<MeshRenderer>().material = mt_normal;
        ProvincesAvailable.Add(provinceClicked);
        ProvincesOccupied.Remove(provinceClicked);

        var script = provinceClicked.transform.GetComponent<TestArea>();
        script.SendMessage("ChessGoAway");
        provinceClicked = null;

        int level = Chess.GetLevel(chess);
        level--;
        if (level == -3)
            level = 10;
        if (level == -2)
            level = 9;

        count++;
        quant[level]++;

        used[IsChildOf(chess)] = false;

        GameObject Number = GameObject.Find("Number").gameObject;
        Number.transform.GetChild(level).GetComponent<Text>().text = quant[level].ToString() + "/" + total[level].ToString();

        var script2 = chess.transform.GetComponent<Chess>();
        script2.SendMessage("Dead");

        chess = null;

        BtnRemove.SetActive(false);
        BtnNext.SetActive(false);
    }
    #endregion

    public static GameObject getChess()
    {
        return chess;
    }

    private static int IsChildOf(GameObject isClickedChess)
    {
        GameObject allChesses = GameObject.Find("Red");
        for(int i = 0; i < allChesses.transform.childCount; i++)
        {
            if (allChesses.transform.GetChild(i).gameObject.name.Equals(isClickedChess.transform.name))
                return i;
        }
        return -1;
    }
}
