using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rival {

    BaseManager _BSM;
    battleJudgement _BTJ;
    BattleManager _BTM;

    bool per_substitude;
    float rival_personal_trust;
    int Scam_data;
    int select;
    int num;

    public int NUM
    {
        get { return num; }
    }

    public int SELECT
    {
        get { return select; }
    }

    public int SCAM_DATA
    {
        get { return Scam_data; }
        set { Scam_data = value; }
    }

    public bool PER_SUB
    {
        get { return per_substitude; }
        set { per_substitude = value; }
    }

    public float RIVALS_TRUST
    {
        get
        {
            float t = _BSM.NOWUSER.PROBITY + rival_personal_trust + _BTM.ITEMACTION.ITEM7;
            if (t > 100.0f) t = 100.0f;
            if (t < 0.0f) t = 0.0f;
            return t;
        }
    }

    Dictionary<string, Dictionary<string, string>> feel;

    public Rival(battleJudgement b)
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BTJ = b;
        _BTM = b.GetComponent<BattleManager>();
        per_substitude = false;
        feel = new Dictionary<string, Dictionary<string, string>>();
        feel = DataMng.instance.Get(LowDataType.PROBABILLITYITEM).NODE;
        rival_personal_trust = Random.Range(-5f, 15f);
        SCAM_DATA = 0;
    }

    void endInit()
    {
        per_substitude = false;
        SCAM_DATA = 0;
    }

    /// <summary>
    /// 라이벌의 플레이(카드 선택)
    /// </summary>
    /// <returns></returns>
    public void Rivals_play()
    {
        if ((_BTM.ITEMUSAGESTATUS & 1) == 0)
        {
            float impulse = (_BTM.ITEMACTION.ITEM6) ? _BTM.ITEMACTION.IMPLUSE_6 : Random.Range(0, 100.0f);
            impulse = (_BSM.INBATTLE.ISnoLIE) ? -1 : impulse;

            if (impulse > RIVALS_TRUST)
            {
                int select = 0;

                int s = (_BTM.ITEMACTION.ITEM6) ? _BTM.ITEMACTION.S_6 : Random.Range(0, 101);
                Debug.Log(s);
                if (s % 2 == 0)
                {
                    select = Scam_feel(_BTM.MYCHOICE);
                    SCAM_DATA += (1 << 0);
                }
                if (s % 3 == 0)
                {
                    Scam_hand();
                    SCAM_DATA += (1 << 1);
                }
                if (s % 10 == 0)
                {
                    PER_SUB = true;
                    SCAM_DATA += (1 << 2);
                }
                if (s % 25 == 0)
                {
                    Scam_SuperCoin();
                    SCAM_DATA += (1 << 3);
                }

                _BTM.YOURCHOICE = select;
            }
            else
            {
                while (true)
                {
                    int ran = Random.Range(0, 3);
                    if (_BTJ.YOURCOINpack[ran] == 0)
                        continue;
                    else
                    {
                        _BTM.YOURCHOICE = ran;
                        break;
                    }
                }
            }
        }
        else
        {
            float impulse = (_BTM.ITEMACTION.ITEM6) ? _BTM.ITEMACTION.IMPLUSE_6 : Random.Range(0, 100.0f);

            if (impulse > RIVALS_TRUST)
            {
                int s = (_BTM.ITEMACTION.ITEM6) ? _BTM.ITEMACTION.S_6 : Random.Range(0, 101);

                if (s % 3 == 0)
                {
                    Scam_hand();
                    SCAM_DATA += (1 << 1);
                }
                if (s % 10 == 0)
                {
                    PER_SUB = true;
                    SCAM_DATA += (1 << 2);
                }
                if (s % 25 == 0)
                {
                    Scam_SuperCoin();
                    SCAM_DATA += (1 << 3);
                }
            }
        }
    }

    /// <summary>
    /// 사기(촉)
    /// </summary>
    int Scam_feel(int i)
    {
        Debug.Log("rival_scam_1");
        int num = Random.Range(0, feel.Count);
        int prob = Random.Range(0, 101);
        if (prob < int.Parse(feel[num.ToString()]["Middle"]))
            return draw(i);
        else if (prob < int.Parse(feel[num.ToString()]["High"]))
            return win(i);
        else
            return lose(i);        
    }

    /// <summary>
    /// 소매넣기
    /// </summary>
    void Scam_hand()
    {
        Debug.Log("rival_scam_2");
        int left = 0; 
        foreach (KeyValuePair<int, int> count in _BTJ.YOURCOINpack)
        {
            if (count.Value == 0)
                left++;
        }
        
        int min = (_BTJ.YOURCOINpack[0] > _BTJ.YOURCOINpack[1]) ? 1 : 0;
        min = (_BTJ.YOURCOINpack[min] > _BTJ.YOURCOINpack[2]) ? 2 : min;
        int max = (_BTJ.YOURCOINpack[0] > _BTJ.YOURCOINpack[1]) ? 0 : 1;
        max = (_BTJ.YOURCOINpack[max] > _BTJ.YOURCOINpack[2]) ? max : 2;

        if (left == 1)
            min = lose(left);

        _BTJ.YOURCOINpack[max]--;
        _BTJ.YOURCOINpack[min]++;
    }

    /// <summary>
    /// 바꿔치기
    /// </summary>
    public void Scam_substitude()
    {        
        if (PER_SUB)
        {
            Debug.Log("rival_scam_3");
            if (_BTJ.YOURCOINpack[win(_BTM.MYCHOICE)] != 0)
                _BTM.YOURCHOICE = win(_BTM.MYCHOICE);
            else if (_BTJ.YOURCOINpack[draw(_BTM.MYCHOICE)] != 0)
                _BTM.YOURCHOICE = draw(_BTM.MYCHOICE);

            PER_SUB = false;
        }
    }

    /// <summary>
    /// 슈퍼코인
    /// </summary>
    void Scam_SuperCoin()
    {
        Debug.Log("rival_scam_4");
        _BTM.ITEMACTION.ITEM10_Y = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void M1S_rivalSelect()
    {
        List<int> clist = new List<int>();
        Dictionary<int, int> pack = new Dictionary<int, int>(_BTJ.YOURCOINpack);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < pack[i];)
            {
                pack[i]--;
                clist.Add(i);
            }
        }

        for (int i = 0; i < clist.Count; i++)
        {
            int a = Random.Range(0, clist.Count);
            if (i != a)
            {
                int it = clist[i];
                clist[i] = clist[a];
                clist[a] = it;
            }
        }

        _BTM.YOURCHOICE = clist[0];
        _BTM.YOURCHOICE2 = clist[1];
    }

    public void rivalselect_2c()
    {
        int score1 = 0, score2 = 0;
        score1 += cal_eachCoin(_BTM.YOURCHOICE, _BTM.MYCHOICE);
        score1 += cal_eachCoin(_BTM.YOURCHOICE, _BTM.MYCHOICE2);

        score2 += cal_eachCoin(_BTM.YOURCHOICE2, _BTM.MYCHOICE);
        score2 += cal_eachCoin(_BTM.YOURCHOICE2, _BTM.MYCHOICE2);

        int r = 2;
        if (score1 == score2)
            r = Random.Range(0, 2);

        if (score1 > score2 || r == 0)
        {
            select = _BTM.YOURCHOICE;
            num = 0;
        }
        else if (score1 < score2 || r == 1)
        {
            select = _BTM.YOURCHOICE2;
            num = 1;
        }
    }

    int cal_eachCoin(int r, int m)
    {
        if (r > m)
        {
            if ((r == 2) && (m == 0))
                return 2;

            return 0;
        }
        else if (r == m)
            return 1;
        else
        {
            if ((r == 0) && (m == 2))
                return 0;

            return 2;
        }
        
    }

    int win(int i)
    {
        switch (i)
        {
            case 0:
                return 2;
            case 1:
                return 0;
            case 2:
                return 1;
            default:
                Debug.Log("에러 : 이기는 카드 선택 불가");
                return -1;
        }
    }
    int lose(int i)
    {
        switch (i)
        {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 0;
            default:
                Debug.Log("에러 : 이기는 카드 선택 불가");
                return -1;
        }
    }
    int draw(int i)
    {
        switch (i)
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 2;
            default:
                Debug.Log("에러 : 이기는 카드 선택 불가");
                return -1;
        }
    }
}
