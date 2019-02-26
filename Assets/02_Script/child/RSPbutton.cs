using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RSPbutton : MonoBehaviour
{

    public enum RSP
    {
        Rock        = 0,
        Sissiors,
        Paper
    }

    RSP _rsp;
    Button _button;
    BattleManager _BTM;
    battleJudgement _BTJ;
    GameObject _clone;
    bool dragOn;
    bool isLeft;

    [SerializeField] GameObject Clone;    

    public bool DRAGON
    {
        set { dragOn = value; }
    }

    public int RSPvalue
    {
        get { return (int)_rsp; }
    }

    public void init(int i, BattleManager BTM)
    {
        if (i >= 3)
        {
            isLeft = true;
            i -= 3;
        }
        else
            isLeft = false;
        
        _rsp = (RSP)i;
        dragOn = false;
        _button = GetComponent<Button>();
        GetComponent<Image>().sprite = ObjectPack.instance.SPRITEPACK["rsp_" + i];
        _BTM = BTM;
        _BTJ = ObjectPack.instance.Get(Managers.BattleManager).GetComponent<battleJudgement>();
    }

    public void closeButton()
    {
        _button.enabled = false;
    }

    public void openButton()
    {
        _button.enabled = true;
    }

    public void onButton()
    {
        if (!_BTM.UNDERDEST_ACTIVE)
        {
            if (_BTJ.MYCOINpack[(int)_rsp] != 0)
            {
                if (!isLeft)
                    _BTM.MYCHOICE = (int)_rsp;
                else
                    _BTM.MYCHOICE2 = (int)_rsp;
                _BTM.EventToSelect();
            }
        }
    }    

    public void Drag_Begin()
    {
        if (dragOn)
        {
            _clone = Instantiate(Clone, GetComponentsInParent<Transform>()[1]);
            _clone.GetComponent<Image>().sprite = ObjectPack.instance.SPRITEPACK["rsp_" + (int)_rsp];
        }
    }

    public void Drag()
    {
        if (dragOn)
        {
            _clone.transform.position = Input.mousePosition;
        }
    }

    public void Drag_End()
    {
        if (_clone != null)
            Destroy(_clone);
    }
}
