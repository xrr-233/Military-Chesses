using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
*/

/// <summary>
/// 
/// </summary>
public class TestArea : MonoBehaviour
{

    private static Material mt_normal, mt_hovering, mt_clicked, mt_canGo, mt_canGoHovering, mt_canEat, mt_canEatHovering;

    private static GameObject isClickedGlobalGameObject;
    private static GameObject isClickedChess;

    private static List<GameObject> Roads;
    private static List<GameObject> Railways;
    private static List<GameObject> Eats;

    private static List<GameObject> RedProvincesOccupied;
    private static List<GameObject> BlueProvincesOccupied;

    private bool isVisited;

    [SerializeField]
    private GameObject chess;

    public List<GameObject> roadsList = new List<GameObject>();
    public List<GameObject> raliwayList = new List<GameObject>();

    private AudioSource sound;
    private AudioClip sd_clicked;
    private AudioClip sd_withdraw;
    private AudioClip[] sd_deploy = new AudioClip[3];
    private AudioClip[] sd_change = new AudioClip[3];
    private AudioClip sd_move;
    private AudioClip sd_win;
    private AudioClip sd_draw;
    private AudioClip sd_lose;

    private static bool isAvailable;
    private static bool isRedRound;


    // Start is called before the first frame update

    private void Awake()
    {
        mt_normal = Resources.Load<Material>("Materials/mt_normal");
        mt_hovering = Resources.Load<Material>("Materials/mt_hovering");
        mt_clicked = Resources.Load<Material>("Materials/mt_clicked");
        mt_canGo = Resources.Load<Material>("Materials/mt_canGo");
        mt_canGoHovering = Resources.Load<Material>("Materials/mt_canGoHovering");
        mt_canEat = Resources.Load<Material>("Materials/mt_canEat");
        mt_canEatHovering = Resources.Load<Material>("Materials/mt_canEatHovering");

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

        isClickedGlobalGameObject = null;
        isVisited = false;
        isAvailable = false;
        isRedRound = true;

        Roads = new List<GameObject>();
        Railways = new List<GameObject>();
        Eats = new List<GameObject>();

        sound = gameObject.AddComponent<AudioSource>();
        sound.playOnAwake = false;
    }

    void Start()
    {
        if(chess != null)
        {
            //print(this.gameObject.name + ": " + this.transform.position.x + "," + this.transform.position.y + "," + this.transform.position.z);
            var script = this.chess.transform.GetComponent<Chess>();
            script.SendMessage("SetPosition", this.transform.position);
        }
            
    }

    private void OnMouseEnter()
    {
        if (!isAvailable || EventSystem.current.IsPointerOverGameObject())
            return;
        if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canEat (Instance)"))
            this.transform.GetComponent<MeshRenderer>().material = mt_canEatHovering;
        else if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canGo (Instance)"))
            this.transform.GetComponent<MeshRenderer>().material = mt_canGoHovering;
        else if (isClickedGlobalGameObject != this.gameObject)
            this.transform.GetComponent<MeshRenderer>().material = mt_hovering;
    }

    private void OnMouseExit()
    {
        if (!isAvailable)
            return;
        if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canEatHovering (Instance)") || this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canEat (Instance)"))
            this.transform.GetComponent<MeshRenderer>().material = mt_canEat;
        else if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canGoHovering (Instance)") || this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canGo (Instance)"))
            this.transform.GetComponent<MeshRenderer>().material = mt_canGo;
        else if (isClickedGlobalGameObject != this.gameObject)
            this.transform.GetComponent<MeshRenderer>().material = mt_normal;
    }

    private void OnMouseDown()
    {
        if (!isAvailable || EventSystem.current.IsPointerOverGameObject())
            return;
        if (DeployRed.Status())
        {
            if(this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canGoHovering (Instance)"))
            {
                sound.clip = sd_deploy[Random.Range(0, 3)];
                sound.Play();

                this.chess = DeployRed.getChess();
                var script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("SetPosition", this.transform.position);
                DeployRed.ClearChoosing(this.gameObject, this.chess);
            }
            else if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canEatHovering (Instance)"))
            {
                sound.clip = sd_change[Random.Range(0, 3)];
                sound.Play();

                var script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("Dead", this.chess);
                DeployRed.UndoClearChoosing(this.gameObject, this.chess);
                this.chess = DeployRed.getChess();
                script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("SetPosition", this.transform.position);
                DeployRed.ClearChoosing(this.gameObject, this.chess);
            }
            else if (this.chess != null)
            {
                sound.clip = sd_clicked;
                sound.Play();

                if (!this.chess.name.Equals("iii_jun_qi"))
                {
                    if(isClickedGlobalGameObject != null)
                        isClickedGlobalGameObject.transform.GetComponent<MeshRenderer>().material = mt_normal;
                    isClickedGlobalGameObject = this.gameObject;
                    this.transform.GetComponent<MeshRenderer>().material = mt_clicked;
                    DeployRed.RemoveAsk(this.chess, this.gameObject);
                }
            }
            return;
        }
        if (DeployBlue.Status())
        {
            if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canGoHovering (Instance)"))
            {
                sound.clip = sd_deploy[Random.Range(0,3)];
                sound.Play();

                this.chess = DeployBlue.getChess();
                var script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("SetPosition", this.transform.position);
                DeployBlue.ClearChoosing(this.gameObject, this.chess);
            }
            else if (this.transform.GetComponent<MeshRenderer>().material.name.Equals("mt_canEatHovering (Instance)"))
            {
                sound.clip = sd_change[Random.Range(0, 3)];
                sound.Play();

                var script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("Dead", this.chess);
                DeployBlue.UndoClearChoosing(this.gameObject, this.chess);
                this.chess = DeployBlue.getChess();
                script = this.chess.transform.GetComponent<Chess>();
                script.SendMessage("SetPosition", this.transform.position);
                DeployBlue.ClearChoosing(this.gameObject, this.chess);
            }
            else if (this.chess != null)
            {
                sound.clip = sd_clicked;
                sound.Play();

                if (!this.chess.name.Equals("iii_jun_qi"))
                {
                    if (isClickedGlobalGameObject != null)
                        isClickedGlobalGameObject.transform.GetComponent<MeshRenderer>().material = mt_normal;
                    isClickedGlobalGameObject = this.gameObject;
                    this.transform.GetComponent<MeshRenderer>().material = mt_clicked;
                    DeployBlue.RemoveAsk(this.chess, this.gameObject);
                }
            }
            return;
        }
        else
        {
            if (Eats.Contains(this.gameObject))
            {
                isClickedGlobalGameObject.transform.GetComponent<MeshRenderer>().material = mt_normal;
                if (isRedRound)
                    RedProvincesOccupied.Add(gameObject);
                else
                    BlueProvincesOccupied.Add(gameObject);

                var script = isClickedGlobalGameObject.transform.GetComponent<TestArea>();
                script.SendMessage("UndoCanGo");
                script.SendMessage("ChessGoAway");
                Eat(isClickedChess, this.chess);

                isRedRound ^= true;
                WarSystem.ChangeSide();
                DarkChess();
                return;
            }
            if (Roads.Contains(this.gameObject) || Railways.Contains(this.gameObject))
            {
                sound.clip = sd_move;
                sound.Play();

                isClickedGlobalGameObject.transform.GetComponent<MeshRenderer>().material = mt_normal;
                if (isRedRound)
                    RedProvincesOccupied.Add(gameObject);
                else
                    BlueProvincesOccupied.Add(gameObject);

                var script = isClickedGlobalGameObject.transform.GetComponent<TestArea>();
                script.SendMessage("UndoCanGo");
                script.SendMessage("ChessGoAway");
                this.chess = isClickedChess;
                var script2 = this.chess.transform.GetComponent<Chess>();
                script2.SendMessage("SetPosition", this.transform.position);
                isClickedGlobalGameObject = null;
                isClickedChess = null;

                isRedRound ^= true;
                WarSystem.ChangeSide();
                DarkChess();
                return;
            }
            if (isClickedGlobalGameObject != this.gameObject)
            {
                sound.clip = sd_clicked;
                sound.Play();

                if (isClickedGlobalGameObject != null)
                {
                    isClickedGlobalGameObject.transform.GetComponent<MeshRenderer>().material = mt_normal;
                    var script = isClickedGlobalGameObject.transform.GetComponent<TestArea>();
                    script.SendMessage("UndoCanGo");
                }
                isClickedGlobalGameObject = this.gameObject;
                this.transform.GetComponent<MeshRenderer>().material = mt_clicked;

                if (this.chess != null)
                {
                    int temp = this.chess.transform.GetComponent<Chess>().GetLevel();
                    if (temp == -1)
                    {
                        print("地雷不可移动！");
                    }
                    else if (temp == -3)
                        print("军旗不可移动！");
                    else if ((isRedRound && chess.transform.GetComponent<Chess>().GetSide() == "Blue") || (!isRedRound && chess.transform.GetComponent<Chess>().GetSide() == "Red"))
                    {
                        print("敌方军棋不可移动！");
                    }
                    else if (temp == 9)
                    {
                        isClickedChess = this.chess;
                        CanGo(100);
                    }
                    else
                    {
                        isClickedChess = this.chess;
                        CanGo(5);
                    }
                }
            }
            else
            {
                sound.clip = sd_withdraw;
                sound.Play();

                this.transform.GetComponent<MeshRenderer>().material = mt_normal;
                var script = isClickedGlobalGameObject.transform.GetComponent<TestArea>();
                script.SendMessage("UndoCanGo");
                isClickedGlobalGameObject = null;
                isClickedChess = null;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region 判定格子是否可走
    private void CanGo(int step)
    {
        //print(this.gameObject + ": " + step);
        if (isVisited)
            return;
        
        if (step == -1)
        {
            if (chess != null)
            {
                string mySide = isClickedChess.transform.GetComponent<Chess>().GetSide();
                string itsSide = chess.transform.GetComponent<Chess>().GetSide();
                if (mySide.Equals(itsSide))
                    return;
                else
                {
                    this.transform.GetComponent<MeshRenderer>().material = mt_canEat;
                    Roads.Add(this.gameObject);
                    Eats.Add(this.gameObject);
                }
                return;
            }
            this.transform.GetComponent<MeshRenderer>().material = mt_canGo;
            Roads.Add(this.gameObject);
            return;
        }
        if (step == 5 || step == 100)
        {
            Railways.Clear();
            Roads.Clear();
            Eats.Clear();
            foreach (GameObject roadTo in roadsList)
            {
                var script = roadTo.transform.GetComponent<TestArea>();
                script.SendMessage("CanGo", -1);
            }
        }
        else if (step != 5 && this.chess != null)
        {
            string mySide = isClickedChess.transform.GetComponent<Chess>().GetSide();
            string itsSide = this.chess.transform.GetComponent<Chess>().GetSide();
            if (mySide.Equals(itsSide))
                return;
            else
            {
                this.transform.GetComponent<MeshRenderer>().material = mt_canEat;
                Railways.Add(this.gameObject);
                Eats.Add(this.gameObject);
                return;
            }
        }
        else
        {
            this.transform.GetComponent<MeshRenderer>().material = mt_canGo;
            Railways.Add(this.gameObject);
            if (step <= 0)
                return;
        }
        isVisited = true;
        foreach(GameObject railwayTo in raliwayList)
        {
            var script = railwayTo.transform.GetComponent<TestArea>();
            script.SendMessage("CanGo", step - 1);
        }
        isVisited = false;
    }

    private void UndoCanGo()
    {
        foreach (GameObject roadTo in Roads)
        {
            roadTo.transform.GetComponent<MeshRenderer>().material = mt_normal;
        }
        Roads.Clear();
        foreach (GameObject railwayTo in Railways)
        {
            railwayTo.transform.GetComponent<MeshRenderer>().material = mt_normal;
        }
        Railways.Clear();
        foreach (GameObject eatTo in Eats)
        {
            eatTo.transform.GetComponent<MeshRenderer>().material = mt_normal;
        }
        Eats.Clear();
    }
    #endregion

    #region 吃子
    private void Eat(GameObject mySide, GameObject itsSide)
    {
        int myLevel = mySide.transform.GetComponent<Chess>().GetLevel();
        int itsLevel = itsSide.transform.GetComponent<Chess>().GetLevel();

        if(myLevel == -2 || itsLevel == -2)
        {
            sound.clip = sd_draw;
            sound.Play();

            WarSystem.RedReduce();
            WarSystem.BlueReduce();

            this.chess = null;
            var script = mySide.transform.GetComponent<Chess>();
            script.SendMessage("Dead", this.chess);
            script = itsSide.transform.GetComponent<Chess>();
            script.SendMessage("Dead", this.chess);
        }
        else if(itsLevel == -1)
        {
            if(myLevel == 9)
            {
                sound.clip = sd_win;
                sound.Play();

                if (mySide.transform.GetComponent<Chess>().GetSide() == "Red")
                    WarSystem.BlueReduce();
                else
                    WarSystem.RedReduce();

                this.chess = mySide;
                var script = mySide.transform.GetComponent<Chess>();
                script.SendMessage("SetPosition", this.transform.position);
                script = itsSide.transform.GetComponent<Chess>();
                script.SendMessage("Dead");
            }
            else
            {
                sound.clip = sd_lose;
                sound.Play();

                if (mySide.transform.GetComponent<Chess>().GetSide() == "Red")
                    WarSystem.RedReduce();
                else
                    WarSystem.BlueReduce();

                this.chess = itsSide;
                var script = mySide.transform.GetComponent<Chess>();
                script.SendMessage("Dead");
            }
        }
        else if(itsLevel == -3)
        {
            sound.clip = sd_win; //本来是完全胜利 现在先用这个代替
            sound.Play();

            if (mySide.transform.GetComponent<Chess>().GetSide() == "Red")
            {
                WarSystem.BlueReduce();
                GameOver.Win(true);
            }
            else
            {
                WarSystem.RedReduce();
                GameOver.Win(false);
            }

            this.chess = mySide;
            var script = mySide.transform.GetComponent<Chess>();
            script.SendMessage("SetPosition", this.transform.position);
            script = itsSide.transform.GetComponent<Chess>();
            script.SendMessage("Dead");

            GameObject.Find("Canvas (7)").GetComponent<Canvas>().enabled = true;
            GameObject.Find("CameraController").GetComponent<CameraControl>().enabled = false;
            GameObject.Find("Canvas (5)").GetComponent<Canvas>().enabled = false;
        }
        else if(myLevel == itsLevel)
        {
            sound.clip = sd_draw;
            sound.Play();

            WarSystem.RedReduce();
            WarSystem.BlueReduce();

            this.chess = null;
            var script = mySide.transform.GetComponent<Chess>();
            script.SendMessage("Dead", this.chess);
            script = itsSide.transform.GetComponent<Chess>();
            script.SendMessage("Dead", this.chess);
        }
        else if(myLevel > itsLevel)
        {
            sound.clip = sd_lose;
            sound.Play();

            if (mySide.transform.GetComponent<Chess>().GetSide() == "Red")
                WarSystem.RedReduce();
            else
                WarSystem.BlueReduce();

            this.chess = itsSide;
            var script = mySide.transform.GetComponent<Chess>();
            script.SendMessage("Dead");
        }
        else
        {
            sound.clip = sd_win;
            sound.Play();

            if (mySide.transform.GetComponent<Chess>().GetSide() == "Red")
                WarSystem.BlueReduce();
            else
                WarSystem.RedReduce();

            this.chess = mySide;
            var script = mySide.transform.GetComponent<Chess>();
            script.SendMessage("SetPosition", this.transform.position);
            script = itsSide.transform.GetComponent<Chess>();
            script.SendMessage("Dead");
        }

        isClickedGlobalGameObject = null;
        isClickedChess = null;
    }
    #endregion

    #region 暗棋
    public void DarkChessInit()
    {
        GetRedList();
        GetBlueList();

        foreach (GameObject province in RedProvincesOccupied)
        {
            GameObject temp = province.GetComponent<TestArea>().GetChess();
            temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        }
        foreach (GameObject province in BlueProvincesOccupied)
        {
            GameObject temp = province.GetComponent<TestArea>().GetChess();
            temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        }
    }
    public static void DarkChess()
    {
        List<GameObject> tempList = new List<GameObject>();
        if (isRedRound)
        {
            tempList.Clear();
            foreach (GameObject province in RedProvincesOccupied)
            {
                GameObject temp = province.GetComponent<TestArea>().GetChess();
                if (temp != null && temp.transform.GetComponent<Chess>().GetSide() == "Red")
                    temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                else
                    tempList.Add(province);
            }
            foreach (GameObject province in tempList)
                RedProvincesOccupied.Remove(province);
            tempList.Clear();
            foreach (GameObject province in BlueProvincesOccupied)
            {
                GameObject temp = province.GetComponent<TestArea>().GetChess();
                if (temp != null && temp.transform.GetComponent<Chess>().GetSide() == "Blue")
                    temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                else
                    tempList.Add(province);
            }
            foreach (GameObject province in tempList)
                BlueProvincesOccupied.Remove(province);
        }
        else
        {
            tempList.Clear();
            foreach (GameObject province in RedProvincesOccupied)
            {
                GameObject temp = province.GetComponent<TestArea>().GetChess();
                if (temp != null && temp.transform.GetComponent<Chess>().GetSide() == "Red")
                    temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                else
                    tempList.Add(province);
            }
            foreach (GameObject province in tempList)
                RedProvincesOccupied.Remove(province);
            tempList.Clear();
            foreach (GameObject province in BlueProvincesOccupied)
            {
                GameObject temp = province.GetComponent<TestArea>().GetChess();
                if (temp != null && temp.transform.GetComponent<Chess>().GetSide() == "Blue")
                    temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                else
                    tempList.Add(province);
            }
            foreach (GameObject province in tempList)
                BlueProvincesOccupied.Remove(province);
        }
    }
    public void RedChessInvisible()
    {
        GetRedList();
        foreach (GameObject province in RedProvincesOccupied)
        {
            GameObject temp = province.GetComponent<TestArea>().GetChess();
            temp.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
        }
    }
    public void UndoRedChessInvisible()
    {
        GetRedList();
        foreach (GameObject province in RedProvincesOccupied)
        {
            GameObject temp = province.GetComponent<TestArea>().GetChess();
            temp.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            temp.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        }
    }
    #endregion

    #region 控制组件可用性
    public static void Open()
    {
        isAvailable = true;
    }

    public static void Close()
    {
        isAvailable = false;
    }

    public void OpenForButton()
    {
        isAvailable = true;
    }
    #endregion

    private void ChessGoAway()
    {
        this.chess = null;
        isClickedGlobalGameObject = null;
    }

    public void GetRedList()
    {
        RedProvincesOccupied = DeployRed.GetList();
    }
    public void GetBlueList()
    {
        BlueProvincesOccupied = DeployBlue.GetList();
    }
    public GameObject GetChess()
    {
        return chess;
    }

}
