using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minus1_select : MonoBehaviour {

    Image img_r;
    Image img_l;

    BattleManager _BTM;
    int select;

    public int SELECT
    {
        get { return select; }
    }

    private void Start()
    {
        select = -1;
        _BTM = GetComponentInParent<BattleManager>();
    }

    public void SetImage(Image img1, Image img2)
    {
        img_r = img1;
        img_l = img2;
    }

    public void buttonR()
    {
        img_l.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
        Time.timeScale = 1f;
        select = _BTM.MYCHOICE;
        _BTM.M1_Select = false;
    }

    public void buttonL()
    {
        img_r.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
        Time.timeScale = 1f;
        select = _BTM.MYCHOICE2;
        _BTM.M1_Select = false;
    }
}
