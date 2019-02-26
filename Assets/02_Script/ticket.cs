using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ticket : MonoBehaviour {
        
    bool isSuper;

    public bool ISSUPER
    {
        get { return isSuper; }
        set { isSuper = value; }
    }

    Image sticker;

    // Update is called once per frame
    public void _sticker (bool bl)
    {
        sticker = GetComponentsInChildren<Image>()[1];
        sticker.enabled = bl;
        if (bl)
        {
            float w = Random.Range(-120f, 120f);
            float h = Random.Range(-24f, 24f);
            sticker.gameObject.GetComponent<RectTransform>().position = new Vector3(540f + w, 960f + h);
        }
	}
}
