using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class item3_RequestWin : MonoBehaviour {

    BaseManager _BSM;
    BattleRequestWin _BRW;
    [SerializeField] RectTransform[] rsp_item3;

    void Start()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BRW = UIManager.instance.MakeWindow(Windows.BattleRequestWin).GetComponent<BattleRequestWin>();
        _BSM.INBATTLE.IWANT = 3;
    }

    public void Rbutton()
    {
        _BSM.INBATTLE.IWANT = 0;
        button_size();
    }

    public void Sbutton()
    {
        _BSM.INBATTLE.IWANT = 1;
        button_size();
    }

    public void Pbutton()
    {
        _BSM.INBATTLE.IWANT = 2;
        button_size();
    }

    void button_size()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_BSM.INBATTLE.IWANT == i)            
                rsp_item3[i].localScale = Vector3.one;
            else
                rsp_item3[i].localScale = Vector3.one * 0.75f;
        }
    }

    public void openWin()
    {
        if (_BSM == null)
            _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();

        _BSM.INBATTLE.IWANT = 3;
        button_size();
    }

    public void OK()
    {
        gameObject.SetActive(false);
    }

    public void Cancle()
    {
        _BSM.INBATTLE.IWANT = 3;
        gameObject.SetActive(false);
        _BRW.button3_cancle();
    }
}
