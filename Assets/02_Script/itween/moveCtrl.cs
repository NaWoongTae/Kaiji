using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {

        iTween.MoveFrom(gameObject, iTween.Hash("x", -22.0f, "y", 7.0f, "time", 5.0f, "oncomplete", "Moveto", "oncompletetarget", gameObject));
	}

    void Moveto()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x",11.0f,"z",15.0f,"time",3.0f,"delay",2.0f,"oncomplete", "moveby", "oncompletetarget",gameObject));
    }

    void moveby()
    {
        iTween.MoveBy(gameObject, iTween.Hash("x", -11.0f, "time", 7.0f, "easetype", iTween.EaseType.easeInExpo));
    }
}
