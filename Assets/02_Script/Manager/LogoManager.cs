using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour {

    BaseManager _BSM;

	// Use this for initialization
	void Start () {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void next()
    {
        _BSM._loadloginScene("LogoScene");
    }
}
