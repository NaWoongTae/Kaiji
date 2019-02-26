using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreinMenu : inMenuParentsClass
{

    [SerializeField] RectTransform buttonParent;
    [SerializeField] GameObject stageButtonPrefab;

    BaseManager _BSM;
    List<GameObject> _btns;
    
    // Use this for initialization
    void Awake()
    {        
        _btns = new List<GameObject>();        
    }

    protected override List<GameObject> Create(float Box_h, float b, float startPos, LowDataType data, GameObject prefab, RectTransform recT, float Full_h = 1260f)
    {
        List<GameObject> btns = base.Create(Box_h, b, startPos, data, prefab, recT, Full_h);

        for (int i = 0; i < btns.Count; i++)
        {
            btns[i].GetComponent<ItemButton>().init(i + 1);
        }

        return btns;
    }    

    public void open_Info(int today)
    {
        if (_BSM == null)
            _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();

        if (_btns.Count == 0)
            _btns = Create(200f, 35f, 470f, LowDataType.ITEM, stageButtonPrefab, buttonParent);

        info_of_coin(today);
        info_of_boss(today);
    }

    void info_of_coin(int today)
    {
        if (_BSM.NOWUSER.COINDAY + 1 <= today)
        {
            _BSM.NOWUSER.COINDAY = today + Random.Range(3, 6);
        }
        else if (_BSM.NOWUSER.COINDAY == 1) { }
        else
        {
            if (_BSM.NOWUSER.COINDAY <= today)
            {
                _btns[7].GetComponent<Button>().enabled = true;
                _BSM.INNEWS.ITEM_COININFO = true;
            }
            else
            {
                _btns[7].GetComponent<Button>().enabled = false;
                _BSM.INNEWS.ITEM_COININFO = false;
            }
            _btns[7].GetComponent<ItemButton>().storeCount();
        }
    }

    void info_of_boss(int today)
    {
        if (_BSM.NOWUSER.BOSSDAY + 1 <= today)
        {
            _BSM.NOWUSER.BOSSDAY = today + Random.Range(1, 4) * 7;
        }
        else if (_BSM.NOWUSER.BOSSDAY == 1)
        {  }
        else
        {
            if (today >= _BSM.NOWUSER.BOSSDAY + 3)
            {
                _btns[8].GetComponent<Button>().enabled = false;
                _BSM.INNEWS.ITEM_BOSSINFO = false;
                _BSM.INNEWS.BOSS_FIGHT = false;
            }
            else if (today >= _BSM.NOWUSER.BOSSDAY)
            {
                _btns[8].GetComponent<Button>().enabled = true;
                _BSM.INNEWS.ITEM_BOSSINFO = true;
                _BSM.INNEWS.BOSS_FIGHT = true;
            }

            _btns[8].GetComponent<ItemButton>().storeCount();
        }
    }

}
