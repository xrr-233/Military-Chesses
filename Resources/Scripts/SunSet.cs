using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*/

/// <summary>
/// 
/// </summary>
public class SunSet : MonoBehaviour
{
    private GameObject DirectionalLight;
    private List<GameObject> CornerPointLight;
    private GameObject CentralPointLight;
    // Start is called before the first frame update
    private void Start()
    {
        CornerPointLight = new List<GameObject>();
        DirectionalLight = transform.GetChild(0).gameObject;
        for (int i = 1; i <= 4; i++)
            CornerPointLight.Add(transform.GetChild(i).gameObject);
            
        CentralPointLight = transform.GetChild(5).gameObject;

    }

    // Update is called once per frame
    private void Update()
    {
        DirectionalLight.transform.Rotate(0, (float)(0.6 * Time.deltaTime), 0, Space.Self);
        if(DirectionalLight.transform.eulerAngles.y > 80.0f && DirectionalLight.transform.eulerAngles.y < 280.0f)
            foreach(GameObject g in CornerPointLight)
                g.transform.GetComponent<Light>().enabled = true;
        else
            foreach (GameObject g in CornerPointLight)
                g.transform.GetComponent<Light>().enabled = false;
        if (DirectionalLight.transform.eulerAngles.y > 90.0f && DirectionalLight.transform.eulerAngles.y < 270.0f)
            CentralPointLight.transform.GetComponent<Light>().enabled = true;
        else
            CentralPointLight.transform.GetComponent<Light>().enabled = false;
    }
}
