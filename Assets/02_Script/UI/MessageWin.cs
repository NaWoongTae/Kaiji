using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWin : MonoBehaviour {

    [SerializeField] Text _txt;

    float t;

    public void Awake()
    {
        t = 1;
        _txt.text = "";
        gameObject.SetActive(false);
    }

    public void OnMessage(string str)
    {        
        _txt.text = str;
        t = Time.timeScale;
        Time.timeScale = 0;
    }

    public void cancel()
    {
        Time.timeScale = t;
        gameObject.SetActive(false);
    }
}
