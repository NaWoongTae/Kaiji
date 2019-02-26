using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToEaseType : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.MoveTo(gameObject, iTween.Hash("y", 15.0f, "time", 4.0f, "delay", 1.0f, "easetype", iTween.EaseType.easeInOutBounce));
	}
}
