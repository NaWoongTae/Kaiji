using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movefrom : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        iTween.MoveFrom(gameObject, iTween.Hash("z", 15.0f, "time", 10.0f, "delay", 3.0f));
    }
}
