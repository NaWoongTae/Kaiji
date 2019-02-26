using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveto : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 8.0f, "time", 3.0f, "delay", 1.0f));
    }
}
