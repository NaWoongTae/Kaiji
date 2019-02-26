using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAction
{

    Dictionary<string, Dictionary<string, string>> nodeDic = new Dictionary<string, Dictionary<string, string>>();
    BattleManager _BTM;
    battleJudgement _BTJ;

    bool item4;
    bool item4_isLoading;    
    float item4_value;

    bool item6;
    float impluse_6; // 아이템6(관찰)을 사용했을때 미리 알게되는 적의 사기 여부
    int s_6; // 사기를 친다고 했을때의 사기 종류

    float item7;

    bool item10;
    bool item10_you;

    public bool ITEM4
    {
        get { return item4; }
        set { item4 = value; }
    }

    public float ITEM4_VALUE
    {
        get { return item4_value; }
        set { item4_value = value; }
    }

    public bool ITEM6
    {
        get { return item6; }
        set { item6 = value; }
    }

    public float IMPLUSE_6
    {
        get { return impluse_6; }
        set { impluse_6 = value; }
    }

    public int S_6
    {
        get { return s_6; }
        set { s_6 = value; }
    }

    public float ITEM7
    {
        get { return item7; }
        set { item7 = value; }
    }

    public bool ITEM10
    {
        get { return item10; }
        set { item10 = value; }
    }

    public bool ITEM10_Y
    {
        get { return item10_you; }
        set { item10_you = value; }
    }

    public bool ITEM4_LOADING
    {
        get { return item4_isLoading; }
        set { item4_isLoading = value; }
    }

    public ItemAction()
    {
        _BTM = ObjectPack.instance.Get(Managers.BattleManager).GetComponent<BattleManager>();
        _BTJ = ObjectPack.instance.Get(Managers.BattleManager).GetComponent<battleJudgement>();
        ITEM4 = false;
        ITEM6 = false;
        ITEM7 = 0.0f;
        ITEM10 = false;
        item4_isLoading = false;
        ITEM10_Y = false;
        ITEM4_VALUE = 1;
    }

    public void Item_1_Action()
    {
        // 가바보 중에 몇종류가 남았는지 확인(최소 1 최대 3)
        // 남은 종류가 들어갈 리스트
        List<int> rsp = new List<int>();
        // 상대가 받은패 분석
        foreach (KeyValuePair<int, int> kv in _BTJ.YOURCOINpack)
        {
            if (!(kv.Value == 0))
            {
                // 개수와 종류 분석
                rsp.Add(kv.Key);
            }            
        }

        // 아이템 1 확률표
        nodeDic = DataMng.instance.Get(LowDataType.PROBABILLITYITEM).NODE;
        int DicIndex = Random.Range(0, nodeDic.Count) + 1; // 그중에 하나 고름

        int choice = Random.Range(0, 101); // 100분의 1
        List<int> order = new List<int>(); // 낼 가능성 높은 순서대로 받아올 리스트 

        switch (rsp.Count)
        {
            case 3: // 남은 카드가 3종류일때
                if (choice < int.Parse(nodeDic[DicIndex.ToString()]["Middle"]))
                { order = Selected(rsp, 1); Debug.Log("3미드"); }
                else if (choice < int.Parse(nodeDic[DicIndex.ToString()]["High"]))
                { order = Selected(rsp, 0); Debug.Log("3탑"); }
                else
                { order = Selected(rsp, 2); Debug.Log("3봇"); }
                break;
            case 2: // 남은 카드가 2종류일때
                if (choice < int.Parse(nodeDic[DicIndex.ToString()]["Middle"]))
                { order = Selected(rsp, 1); Debug.Log("2미드"); }
                else
                { order = Selected(rsp, 0); Debug.Log("2탑"); }
                break;
            case 1:
                { order = Selected(rsp, 0); Debug.Log("1종류"); }
                break;
            default: // 남은 카드가 1종류일때
                Debug.Log("ItemAction 오류");
                break;
        }

        int m = int.Parse(nodeDic[DicIndex.ToString()]["Middle"]);
        int h = int.Parse(nodeDic[DicIndex.ToString()]["High"]) - m;
        int l = int.Parse(nodeDic[DicIndex.ToString()]["Low"]) - h - m;

        List<string> str = new List<string>() { "", "", "" };

        str[0] = StringToInt(order[0]) + " : " + h + "%";
        if (order.Count >= 2)
            str[1] = StringToInt(order[1]) + " : " + m + "%";
        if (order.Count >= 3)
            str[2] = StringToInt(order[2]) + " : " + l + "%";

        _BTM.percenTextIn(str[0], str[1], str[2]);
    }

    // 매개변수 : 남은 가지수 , 확률 싸움으로 받아온 패(아직 알수없음) = 00의 확률을 가진 패를 내겠다.
    public List<int> Selected(List<int> rsp, int highest)
    {
        // [묵찌빠][가장높은확률,중간,낮은확률]
        int[][] think = new int[][] { new int[] { 0, 1, 2 }, new int[] { 1, 2, 0 }, new int[] { 2, 0, 1 } };
        int inSelect = Random.Range(0, rsp.Count); // 남은 가지수에서 고른다
        List<int> order = new List<int>();

        if (rsp.Count == 2) // 남은 가지수가 2개일때
        {
            if (rsp.Contains(think[rsp[inSelect]][0]) && !rsp.Contains(think[rsp[inSelect]][1])) // 선택된 표에서 첫번째는 있는데 두번째가 없는패인 경우 두번째를 세번째랑 같게 해준다.
                think[rsp[inSelect]][1] = think[rsp[inSelect]][2];
            else if (rsp.Contains(think[rsp[inSelect]][0]) && !rsp.Contains(think[rsp[inSelect]][2])) // 선택된 표에서 첫번째와 두번째가 있는데 세번째가 없는패인 경우 두번째를 세번째랑 같게 해준다.
                think[rsp[inSelect]][2] = think[rsp[inSelect]][1];
        }
        else if (rsp.Count == 1)
        {
            _BTM.YOURCHOICE = think[rsp[inSelect]][0];
            order.Add(rsp[inSelect]);
            return order;
        }
        // △확률
        // ▽실제 패
        Debug.Log("결과"+ think[rsp[inSelect]][rsp[highest]]+"선택"+ inSelect+"확률"+ highest);
        int final = think[rsp[inSelect]][rsp[highest]]; // think[남은 가지수 중에서의 패][그 패로 시작하는 표에서의 선택된 확률]    
        _BTM.YOURCHOICE = final;
        _BTJ.YOURCOINpack[final]--;              

        order.Add(rsp[inSelect]);
        if (rsp.Count >= 2)
            order.Add(think[rsp[inSelect]][1]);
        if (rsp.Count >= 3)
            order.Add(think[rsp[inSelect]][2]);

        return order;
    }

    public string StringToInt(int i)
    {
        switch (i)
        {
            case 0:
                return "바위";
            case 1:
                return "가위";
            case 2:
                return "보";
        }
        return "오류";
    }

    public void Item_2_Action()
    {
        _BTM.underDeskWin(true);
        Item2_UnderDesk UD = _BTM.GetComponentInChildren<Item2_UnderDesk>();
        UD.colorAndEnable();
    }
    
    public void Item_5_Action(bool inf)
    {
        if (inf)
            ITEM4_VALUE = 2.0f;
        else
            ITEM4_VALUE = 1.0f;
    }

    public void Item_6_Action()
    {
        Debug.Log("아이템6 시작");
        ITEM6 = true;

        IMPLUSE_6 = Random.Range(0, 100.0f);
        string str = "";

        if (IMPLUSE_6 > _BTJ.RIVAL.RIVALS_TRUST)
        {
            S_6 = Random.Range(0, 101);
            Debug.Log(S_6);
            if (S_6 % 25 == 0)
                str = "이 녀석.. 배급표에 황금스티커 붙이는거 같은데..?";
            else if (S_6 % 10 == 0)
                str = "바꿔치기 준비를 하려는듯 자세를 고쳐잡는다..";
            else if (S_6 % 3 == 0)
                str = "책상 밑에서 뭘 꼼지락대나.. 패 바꾸고 있는거 같다.";
            else if (S_6 % 2 == 0)
                str = "내가 뭐낼려는지 알아보려고 기를 쓰고있군..";
            else
                str = "뭘 하려다가 말은 것같다...";
        }
        else
            str = "딱히 뭘 하려는 것 같진않다..";

        _BTM.WATCH = str;
        Debug.Log("아이템6 끝");
    }

    public void Item_7_Action()
    {
        ITEM7 = 15.0f;
    }

    public void Item_10_Action()
    {
        ITEM10 = true;
    }
}
