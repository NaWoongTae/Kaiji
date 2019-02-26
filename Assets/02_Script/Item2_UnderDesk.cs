using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item2_UnderDesk : MonoBehaviour//, IDropHandler
{
    [SerializeField] Image img;
    int TakeNum;
    BaseManager _BSM;
    BattleManager _BTM;
    battleJudgement _BTJ;

    public int RSP
    {
        get { return TakeNum; }
    }

	// Use this for initialization
	void Start ()
    {
        TakeNum = 0;
        img.sprite = ObjectPack.instance.SPRITEPACK["rsp_" + TakeNum];
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BTM = GetComponentInParent<BattleManager>();
        _BTJ = GetComponentInParent<battleJudgement>();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log("드롭1");
    //}

    public void Dropped(BaseEventData eventData)
    {
        int giveNum = eventData.selectedObject.GetComponent<RSPbutton>().RSPvalue;
        _BTJ.MYCOINpack[giveNum]--;
        _BTJ.MYCOINpack[TakeNum]++;
        _BTM.MYCHOICE = -1;
        _BTM.EventToSelect();
        for (int i = 0; i < 3; i++)
            _BTM.RSPBUTTON[i].DRAGON = false;
        
        gameObject.SetActive(false);
    }

    public void Next()
    {
        if (TakeNum == 0)
            TakeNum = 2;
        else if (TakeNum > 0)
            TakeNum--;
        img.sprite = ObjectPack.instance.SPRITEPACK["rsp_" + TakeNum];
        colorAndEnable();
    }

    public void colorAndEnable()
    {
        for (int i = 0; i < 3; i++)
        {
            if (RSP == i)
            {
                _BTM.RSPBUTTON[i].GetComponent<Image>().color = Color.black;
                _BTM.RSPBUTTON[i].DRAGON = false;
            } 
            else if (_BTJ.MYCOINpack[i] == 0)
            {
                _BTM.RSPBUTTON[i].GetComponent<Image>().color = Color.gray;
                _BTM.RSPBUTTON[i].DRAGON = false;
            }
            else
            {
                _BTM.RSPBUTTON[i].GetComponent<Image>().color = Color.white;
                _BTM.RSPBUTTON[i].DRAGON = true;
            }
        }
    }

    public void Cancle()
    {
        gameObject.SetActive(false);
        _BSM.NOWUSER.ITEM[1]++;
        _BTM.ButtonETC_Reset();
        _BTM.IBB[1].textUpdate();

        _BTJ.SUSPICION -= _BTJ.SUSPECT_NUM / _BTM.ITEMACTION.ITEM4_VALUE;

        for (int i = 0; i < 3; i++)
            _BTM.RSPBUTTON[i].DRAGON = false;

        _BTM.ITEMUSAGESTATUS &= (~1);
    }

}
