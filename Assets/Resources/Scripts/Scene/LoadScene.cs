using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private Vector3 center, cityScale, cityTranslate;
    private Vector2 textureTiling;
    private float planeX, planeZ, bottom, top, left, right, bottom_ = -55, top_ = 72, left_ = -180, right_ = 180;
    private float k, equator;
    private GameObject terrain;
    private List<string> infoAll;
    private List<GameObject> cities;
    private Material mt_normal, mt_hovering, mt_clicked, mt_canGo, mt_canGoHovering, mt_canEat, mt_canEatHovering, mt_road, mt_rail, mt_cruise;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SinoJapaneseWar();
    }

    private List<string> ReadFile(string path)
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
        List<string> arrlist = new List<string>();
        while ((line = sr.ReadLine()) != null)
            arrlist.Add(line);

        sr.Close();
        sr.Dispose();
        return arrlist;
    }

    private void InitScene()
    {
        terrain = GameObject.Find("Terrain");
        center = terrain.transform.position;
        planeX = terrain.GetComponent<Terrain>().terrainData.size.x;
        planeZ = terrain.GetComponent<Terrain>().terrainData.size.z;
        cityScale = new Vector3(0.05f, 0.05f, 0.05f);
        cityTranslate = new Vector3(0, 0.05f, 0);
        textureTiling = new Vector2(1, 1);
        cities = new List<GameObject>();

        bottom = center.z + planeZ * 0.26f;
        top = center.z + planeZ * 0.74f;
        left = center.x;
        right = center.x + planeX;

        k = (top - bottom) / (Mathf.Abs(Mercator(top_)) + Mathf.Abs(Mercator(bottom_)));
        equator = bottom + (top - bottom) / (1 + Mathf.Abs(Mercator(top_)) / Mathf.Abs(Mercator(bottom_)));

        mt_normal = Resources.Load<Material>("Materials/mt_normal");
        mt_hovering = Resources.Load<Material>("Materials/mt_hovering");
        mt_clicked = Resources.Load<Material>("Materials/mt_clicked");
        mt_canGo = Resources.Load<Material>("Materials/mt_canGo");
        mt_canGoHovering = Resources.Load<Material>("Materials/mt_canGoHovering");
        mt_canEat = Resources.Load<Material>("Materials/mt_canEat");
        mt_canEatHovering = Resources.Load<Material>("Materials/mt_canEatHovering");
        mt_road = Resources.Load<Material>("Materials/mt_road");
        mt_rail = Resources.Load<Material>("Materials/mt_rail");
        mt_cruise = Resources.Load<Material>("Materials/mt_cruise");
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
        Vector3 returnVector = new Vector3(0, 30, 0);
        float x = left + (latitude - left_) / (right_ - left_) * (right - left);
        float z = equator + k * Mercator(longitude);
        returnVector.x = x + 7f;
        returnVector.z = z;
        Ray ray = new Ray(returnVector, -terrain.transform.up);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        returnVector.y = hit.point.y + 0.3f;
        return returnVector;
    }

    private GameObject FindCity(string city)
    {
        int l = 0, r = cities.Count - 1, mid;
        while(l < r)
        {
            mid = (l + r) >> 1;
            if (city.CompareTo(cities[mid].name) == 0)
                return cities[mid];
            else if (city.CompareTo(cities[mid].name) == 1)
                r = mid - 1;
            else
                l = mid + 1;
        }
        return cities[l];
    }

    private float GetDistance(GameObject a, GameObject b)
    {
        return Mathf.Sqrt((a.transform.position.x - b.transform.position.x) * (a.transform.position.x - b.transform.position.x) +
                          (a.transform.position.y - b.transform.position.y) * (a.transform.position.y - b.transform.position.y) +
                          (a.transform.position.z - b.transform.position.z) * (a.transform.position.z - b.transform.position.z));
    }

    public void SinoJapaneseWar()
    {
        string scene = "Sino-Japanese War";
        if(SceneManager.GetActiveScene().name != scene)
            SceneManager.LoadScene(scene);
        InitScene();

        infoAll = ReadFile(string.Format("{0}/Resources/Texts/{1}.txt", Application.dataPath, scene));

        GameObject cities_gameObject = new GameObject("Cities");
        GameObject routes = new GameObject("Routes");
        string status = null;
        foreach (string line in infoAll)
        {
            if (line == "Cities" || line == "Railways" || line == "Cruise")
                status = line;
            else if (line == "Roads")
            {
                IComparer<GameObject> comparer = new MyCompare();
                cities.Sort(comparer);
                status = line;
            }
            else if (status == "Cities")
            {
                string[] city = line.Split('\t');
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Plane);
                g.name = city[0];
                g.transform.position = GlobalToMap(float.Parse(city[1]), float.Parse(city[2]));
                g.transform.localScale = cityScale;
                g.GetComponent<MeshRenderer>().material = mt_normal;
                g.transform.parent = cities_gameObject.transform;
                cities.Add(g);
            }
            else if (status == "Roads" || status == "Railways" || status == "Cruise")
            {
                string[] road = line.Split('\t');
                GameObject st = FindCity(road[0]);
                GameObject ed = FindCity(road[1]);
                GameObject route_line = new GameObject();
                LineRenderer temp = route_line.AddComponent<LineRenderer>();
                if (status == "Roads")
                {
                    temp.material = mt_road;
                    route_line.name = "Road";
                }
                else if (status == "Railways")
                {
                    temp.material = mt_rail;
                    route_line.name = "Railway";
                }
                else
                {
                    temp.material = mt_cruise;
                    route_line.name = "Cruise";
                }
                temp.SetPosition(0, st.transform.position);
                temp.SetPosition(1, ed.transform.position);
                temp.startWidth = 0.2f;
                temp.endWidth = 0.2f;
                route_line.transform.Rotate(route_line.transform.right, 90);
                temp.alignment = LineAlignment.TransformZ;
                textureTiling.x = GetDistance(st, ed);
                temp.material.SetTextureScale("_MainTex", textureTiling);
                route_line.transform.parent = routes.transform;
            }
        }

        foreach (GameObject city in cities)
            city.transform.Translate(cityTranslate);
    }
}
