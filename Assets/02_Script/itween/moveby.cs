using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveby : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        iTween.MoveBy(gameObject, iTween.Hash("z", 10.0f, "y", 4.0f, "time", 3.0f));
    }
}