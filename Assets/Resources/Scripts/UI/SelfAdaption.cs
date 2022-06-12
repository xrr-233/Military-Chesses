using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfAdaption : MonoBehaviour
{
    private Vector3[] corners = new Vector3[4];
    private Vector2 canvasSize;
    private float width, height;

    public int heightScale, widthScale;
    public enum Mode { xChangeWithY, yChangeWithX }
    public Mode mode;

    // Start is called before the first frame update
    void Start()
    {
        if (heightScale <= 0 || widthScale <= 0)
            Debug.LogError("±ÈÀý³ßÊäÈë´íÎó£¡");
        else
        {
            transform.GetComponent<RectTransform>().GetLocalCorners(corners);
            width = corners[2].x - corners[0].x;
            height = corners[2].y - corners[0].y;
            if (mode == Mode.xChangeWithY)
                width = height / heightScale * widthScale;
            else
                height = width / widthScale * heightScale;
            transform.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            transform.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            canvasSize = transform.GetComponent<RectTransform>().sizeDelta;
            canvasSize.x = width;
            canvasSize.y = height;
            // print(canvasSize);
            gameObject.GetComponent<RectTransform>().sizeDelta = canvasSize;
        }
    }
}
