using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MOD
{
    Newby       = 1,
    Room,
    Match,
    NoFake,
    Minus_1,
    Boss,

    Max
}

public class battleJudgement : MonoBehaviour
{
    List<int> _packed = new List<int>();

    Dictionary<int, int> myCoin;
    Dictionary<int, int> yourCoin;

    float suspicion;
    float last_suspicion;

    float suspect_Num;

    BaseManager _BSM;
    BattleManager _BTM;
    Rival rival;

    public Rival RIVAL
    {
        get { return rival; }
        set { rival = value; }
    }

    public Dictionary<int, int> MYCOINpack
    {
        get { return myCoin; }
        set { myCoin = value; }
    }

    public Dictionary<int, int> YOURCOINpack
    {
        get { return yourCoin; }
        set { yourCoin = value; }
    }

    public List<int> PACKED
    {
        get { return _packed; }
    }

    //의심도%
    public float SUSPICION
    {
        get { return suspicion; }
        set { suspicion = value; }
    }

    public float LAST_SUSPICION
    {
        get { return last_suspicion; }
        set { last_suspicion = value; }
    }

    public float SUSPECT_NUM
    {
        get { return suspect_Num; }
        set { suspect_Num = value; }
    }

    public void Start()
    {
        rival = new Rival(this);
        suspicion = 0;
        suspect_Num = 10;
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BTM = GetComponent<BattleManager>();
        _coinSubmit();
        CoinDivision();
        GetComponent<BattleManager>().EventToSelect();
    }    

    // 스타트에 붙어있음
    //
    public void _coinSubmit()
    {        
        int r = _BSM.INBATTLE.BET_COIN[0];
        int s = _BSM.INBATTLE.BET_COIN[1];
        int p = _BSM.INBATTLE.BET_COIN[2];

        for (int i = 0; i < r + s + p;)
        {
            if (r > 0)
            {
                _packed.Add(0);
                r--;
            }
            if (s > 0)
            {
                _packed.Add(1);
                s--;
            }
            if (p > 0)
            {
                _packed.Add(2);
                p--;
            }
        }
        _rivalSubmit();        
    }

    // 상대방 코인 제출
    void _rivalSubmit()
    {
        float rivalBet = 0;
        int rivalWillBet = Random.Range(_BSM.INBATTLE.LIMIT[0], _BSM.INBATTLE.LIMIT[1]);
        while ((int)rivalBet < rivalWillBet)
        {
            int rand_RSP = Random.Range(0, 3);
                        
            rivalBet += _BSM.PRICE[rand_RSP];
            _packed.Add(rand_RSP);
            _BSM.INBATTLE.YOUR_BET[rand_RSP]++;
        }
    }

    // 팩 셔플
    void _suffleCoin()
    {
        for (int i = 0; i < _packed.Count; i++)
        {
            int j = Random.Range(0, _packed.Count);

            int tmp = _packed[i];
            _packed[i] = _packed[j];
            _packed[j] = tmp;
        }
    }

    // 베팅후
    // 코인 분배 함수
    void CoinDivision()
    {
        myCoin = new Dictionary<int, int> { { 0, 0 }, { 1, 0 }, { 2, 0 } };
        yourCoin = new Dictionary<int, int>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };

        int individual = PACKED.Count / 3;

        if (_BSM.INBATTLE.ISMINUS1)
            individual -= (individual % 2 == 1) ? 1 : 0;

        int take_num = individual / 10 + 1;
        int hand_num = 0;
        
        if (_BSM.INBATTLE.ITEM3)
        {
            SUSPICION += suspect_Num;
            while (take_num > hand_num)
            {
                int i = 0;
                for (i = 0; i < PACKED.Count; i++)
                {
                    if (PACKED[i] == _BSM.INBATTLE.IWANT)
                    {
                        myCoin = organize(myCoin, PACKED[i]);
                        PACKED.RemoveAt(i);
                        hand_num++;
                        break;
                    }
                }
                if (i >= PACKED.Count - 1)
                    break;
            }
            item3_message(_BSM.INBATTLE.IWANT,hand_num);
        }

        _suffleCoin();

        for (int i = 0; i < PACKED.Count; i++)
        {
            if (i < individual-hand_num)
            {
                myCoin = organize(myCoin, PACKED[i]);
            }
            else if (i < individual * 2)
            {
                yourCoin = organize(yourCoin, PACKED[i]);                
            }
        }        
    }

    // 분배받는 카드를 종류별로 구분
    Dictionary<int, int> organize(Dictionary<int, int> dic, int i)
    {
        switch (i)
        {
            case 0:
                dic[0]++;
                break;
            case 1:
                dic[1]++;
                break;
            case 2:
                dic[2]++;
                break;
        }
        return dic;
    }

    void item3_message(int rsp,int num)
    {
        string str = "";
        switch (rsp)
        {
            case 0:
                str = "바위";
                break;
            case 1:
                str = "가위";
                break;
            case 2:
                str = "보";
                break;
        }

        if (num == 0)
            _BSM.ShowMessage("기껏 사기쳤는데 아쉽게도 " + str + "가 없었다.");
        else
            _BSM.ShowMessage(str + "을 " + num + "개 넘겨받았다.");
    }

    public void suspect_superposition()
    {
        if (SUSPICION != 0)
        {
            LAST_SUSPICION += SUSPICION;
            SUSPICION = 0;
        }
        else
        {
            LAST_SUSPICION = LAST_SUSPICION / 3;
            _BSM.NOWUSER.PROBITY += 0.1f * _BTM.ITEMACTION.ITEM4_VALUE;            
        }
    }

    public void suspect_increase()
    {        
        SUSPICION += SUSPECT_NUM / _BTM.ITEMACTION.ITEM4_VALUE;        
    }

    public void Bot_Scolding()
    {
        float per = 0;
        per = LAST_SUSPICION + (100 - LAST_SUSPICION) * ((100 - _BSM.NOWUSER.PROBITY) / 100);
        int i = Random.Range(0, 1000);
        if (i < (int)(per * 10))
        {
            _BSM.ShowMessage("너.. 이 씹새끼 구라치다 걸리면 손모가지 날라가는거 알지??");
        }
    }
}
