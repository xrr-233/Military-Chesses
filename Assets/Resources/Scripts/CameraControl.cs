using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*/

/// <summary>
/// 
/// </summary>
public class CameraControl : MonoBehaviour
{
    private float player_speed = 5.0f; //角色移动速度
    private float mouse_speed = 2.0f;

    private float horizontal;//按下A、D或左右键会从零开始慢慢到-1或1，用来使角色移动更加平滑
    private float vertical;

    private Vector3 lastRotation, lastPosition;
    private GameObject MainCamera;
    void Start()
    {
        MainCamera = transform.GetChild(0).gameObject;
        lastRotation = MainCamera.transform.eulerAngles;
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //旋转视角
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * mouse_speed, 0);
            MainCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * mouse_speed, 0, 0);
            if (!(MainCamera.transform.eulerAngles.x >=0 && MainCamera.transform.eulerAngles.x <= 80))
                MainCamera.transform.eulerAngles = lastRotation;
            else
                lastRotation = MainCamera.transform.eulerAngles;
                
        }
        
        //水平垂直移动
        transform.Translate(MainCamera.transform.right * player_speed * Time.deltaTime * horizontal, Space.World);
        transform.Translate(MainCamera.transform.forward * player_speed * Time.deltaTime * vertical, Space.World);

        #region 限制第一人称视角范围
        if (transform.position.y < 6.0f)
        {
            Vector3 temp = new Vector3(transform.position.x, 6.0f, transform.position.z);
            transform.position = temp;
        }
        if (transform.position.y > 30.0f)
        {
            Vector3 temp = new Vector3(transform.position.x, 30.0f, transform.position.z);
            transform.position = temp;
        }
        /*if (transform.position.x < center.x)
        {
            Vector3 temp = new Vector3(center.x, transform.position.y, transform.position.z);
            transform.position = temp;
        }
        if (transform.position.x > center.x + planeX)
        {
            Vector3 temp = new Vector3(center.z, transform.position.y, transform.position.z);
            transform.position = temp;
        }
        if (transform.position.z < center.z + planeZ / 3)
        {
            Vector3 temp = new Vector3(transform.position.x, transform.position.y, -4.0f);
            transform.position = temp;
        }
        if (transform.position.z > center.z + planeZ / 3 * 2)
        {
            Vector3 temp = new Vector3(transform.position.x, transform.position.y, 8.0f);
            transform.position = temp;
        }*/
        #endregion
    }
}
