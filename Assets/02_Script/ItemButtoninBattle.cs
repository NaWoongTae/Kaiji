using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtoninBattle : MonoBehaviour {

    Dictionary<string, Dictionary<string, string>> itemData = new Dictionary<string, Dictionary<string, string>>();
    
    Text[] txt;
    int num = 0;
    BattleManager _BTM;
    BaseManager _BSM;
    battleJudgement _BTJ;
     //= new ItemAction(ObjectPack.instance.Get(Managers.BattleManager).GetComponent<BattleManager>());

    bool isUsed;
    int UsedTurn;
    int ThisDuration;

    public int NUM
    {
        get { return num; }
    }

    public bool ISUSED
    {
        get { return isUsed; }
        set { isUsed = value; }
    }

    private void Awake()
    {
        isUsed = false;
        UsedTurn = 0;
        
        itemData = DataMng.instance.Get(LowDataType.ITEM).NODE;        
        _BTM = GetComponentInParent<BattleManager>();
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BTJ = GetComponentInParent<battleJudgement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init(int i)
    {
        num = i;
        txt = GetComponentsInChildren<Text>();
        Setting();
    }

    void Setting()
    {
        txt[0].text = itemData[num.ToString()][(Item.Index.Name).ToString()];
        txt[1].text = _BSM.NOWUSER.ITEM[num - 1].ToString();
        if (num == 8 || num == 9)
        {
            isUsed = true;
            GetComponent<Image>().color = Color.gray;
            GetComponent<Button>().enabled = false;
        }
        ThisDuration = int.Parse(itemData[num.ToString()][(Item.Index.Duration).ToString()]);
    }

    public void UsedItem()
    {
        if (_BSM.NOWUSER.ITEM[num - 1] >= 0)
        {
            if (!isUsed)
            {
                UsedTurn = _BTM.TURN;
                isUsed = true;
                _BTM.ITEMUSAGESTATUS |= (1 << (num - 1));
                _BSM.NOWUSER.ITEM[num - 1]--;
                txt[1].text = _BSM.NOWUSER.ITEM[num - 1].ToString();
                GetComponent<Image>().color = new Color(0.8f, 0.8f, 1f);

                switch (num)
                {
                    case 1:
                        Debug.Log("촉");
                        _BTM.ITEMACTION.Item_1_Action();                        
                        break;
                    case 2:
                        Debug.Log("소매넣기");
                        _BTM.ITEMACTION.Item_2_Action();
                        break;
                    case 4:
                        Debug.Log("손 > 눈");
                        _BTM.ITEMACTION.ITEM4 = true;
                        break;
                    case 5:
                        Debug.Log("매수");
                        _BTM.ITEMACTION.Item_5_Action(true);
                        break;
                    case 6:
                        Debug.Log("관찰");
                        _BTM.ITEMACTION.Item_6_Action();
                        break;
                    case 7:
                        Debug.Log("쿠사리");
                        _BTM.ITEMACTION.Item_7_Action();
                        break;
                    case 10:
                        Debug.Log("슈퍼코인");
                        _BTM.ITEMACTION.Item_10_Action();
                        break;
                }
                _BSM.NOWUSER.PROBITY -= float.Parse(itemData[num.ToString()][(Item.Index.Cost).ToString()]) / _BTM.ITEMACTION.ITEM4_VALUE;
                if (num != 10)
                    _BTJ.suspect_increase();
            }
            else
                _BSM.ShowMessage("이미 사용한 사기입니다");
        }
        else
            _BSM.ShowMessage("사기를 다 쳤어");
    }

    public void CheckItemDuration()
    {
        if (_BTM.TURN - UsedTurn >= ThisDuration)
        {
            isUsed = false;

            if (num == 4)
                _BTM.ITEMACTION.ITEM4 = false;
            else if (num == 5)
                _BTM.ITEMACTION.Item_5_Action(ISUSED);
            else if (num == 6)
                _BTM.ITEMACTION.ITEM6 = false;
            else if (num == 7)
                _BTM.ITEMACTION.ITEM7 = 0.0f;
        }

        if (ISUSED)
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 1f);
        else
            GetComponent<Image>().color = Color.white;
    }

    public void textUpdate()
    {
        isUsed = false;
        txt[1].text = _BSM.NOWUSER.ITEM[num - 1].ToString();
    }
}
