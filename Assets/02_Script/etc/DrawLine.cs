using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    private RectTransform imageRectTransform;
    private Image _image;

    float lineWidth = 2.0f;
    Vector2 pointA;
    Vector2 pointB;

    // Use this for initialization
    void Awake()
    {
        pointA = new Vector2(0, 0);
        pointB = new Vector2(0, 0);

        imageRectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void init(Vector2 start, Vector2 end, float _lineWidth = 5.0f)
    {
        pointA = start;
        pointB = end;
        lineWidth = _lineWidth;
        DrawGraph();
    }

    void DrawGraph()
    {
        //_image.color = Color.red;
        Vector2 differenceVector = pointB - pointA;
        _image.color = (differenceVector.y == 0f) ? Color.red : ((differenceVector.y > 0f) ? Color.red : Color.blue);

        imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        imageRectTransform.position = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        imageRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }    
}
