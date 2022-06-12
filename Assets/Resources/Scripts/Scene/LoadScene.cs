using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private Vector3 center;
    private float planeX, planeZ, bottom, top, left, right, bottom_ = -56, top_ = 72, left_ = -180, right_ = 180;
    private float k, equator;
    private ArrayList infoAll;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SinoJapaneseWar();
    }

    private ArrayList ReadFile(string path)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path);
        }
        catch
        {
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
            arrlist.Add(line);

        sr.Close();
        sr.Dispose();
        return arrlist;
    }

    private void InitScene()
    {
        center = GameObject.Find("Terrain").transform.position;
        planeX = GameObject.Find("Terrain").transform.GetComponent<Terrain>().terrainData.size.x;
        planeZ = GameObject.Find("Terrain").transform.GetComponent<Terrain>().terrainData.size.z;

        bottom = center.z + planeZ * 0.26f;
        top = center.z + planeZ * 0.74f;
        left = center.x;
        right = center.x + planeX;

        k = (top - bottom) / (Mathf.Abs(Mercator(top_)) + Mathf.Abs(Mercator(bottom_)));
        print(k);
        equator = bottom + (top - bottom) / (1 + Mathf.Abs(Mercator(top_)) / Mathf.Abs(Mercator(bottom_)));
    }

    private float Mercator(float angle)
    {
        if (angle >= 0)
            return Mathf.Log(Mathf.Tan(Mathf.Deg2Rad * (45 + Mathf.Abs(angle / 2))));
        else
            return -Mathf.Log(Mathf.Tan(Mathf.Deg2Rad * (45 + Mathf.Abs(angle / 2))));
    }

    private Vector3 GlobalToMap(float longitude, float latitude)
    {
        return new Vector3(left + (latitude - left_) / (right_ - left_) * (right - left), 3.5f, equator + k * Mercator(longitude));
    }

    public void SinoJapaneseWar()
    {
        string scene = "Sino-Japanese War";
        if(SceneManager.GetActiveScene().name != scene)
            SceneManager.LoadScene(scene);
        InitScene();

        infoAll = ReadFile(string.Format("{0}/Resources/Texts/{1}.txt", Application.dataPath, scene));

        GameObject cities = new GameObject("Cities");
        string status = null;
        foreach (string line in infoAll)
        {
            if (line == "Cities")
                status = line;
            else if (status == "Cities")
            {
                string[] city = line.Split(' ');
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Plane);
                g.name = city[0];
                g.transform.position = GlobalToMap(float.Parse(city[1]), float.Parse(city[2]));
            }
        }
    }
}
