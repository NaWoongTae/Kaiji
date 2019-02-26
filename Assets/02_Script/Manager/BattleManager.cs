using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum result
{
    win,
    drow,
    lose
}

public class BattleManager : MonoBehaviour
{
    // prefab으로 만들자
    [SerializeField] Image ticket;
    [SerializeField] Button submit;
    [SerializeField] GameObject stack;
    [SerializeField] GameObject underDesk;

    [SerializeField] Text[] score;
    [SerializeField] GameObject rspButtonPrefab;    
    [SerializeField] Transform[] rsp_RL;
    [SerializeField] Text[] precenText;

    [SerializeField] GameObject mul5;
    [SerializeField] Transform mul5_parent;
    [SerializeField] Image waitBar;
    [SerializeField] Text watch;
    [SerializeField] Image _lock;
    [SerializeField] GameObject minus1_select;

    List<Sprite> tickets;
    List<GameObject> usedPack;
    List<Text> leftCount;
    List<RSPbutton> rspButton;
    List<ItemButtoninBattle> ibb;

    ItemAction _itemAction;

    int myChoice;
    int yourChoice;

    int myChoice2;
    int yourChoice2;

    int myscore;
    int enemyscore;
    stampWin stamp;
    Minus1_select M1_S;
    int run;
    int turn;
    int ItemUsageStatus;
    int[] leftcount_array;

    IEnumerator coroutin;

    public ItemAction ITEMACTION
    {
        get { return _itemAction; }
        set { _itemAction = value; }
    }

    public int ITEMUSAGESTATUS
    {
        get { return ItemUsageStatus; }
        set { ItemUsageStatus = value; }
    }

    public bool UNDERDEST_ACTIVE
    {
        get { return underDesk.activeSelf; }
    }

    public List<RSPbutton> RSPBUTTON
    {
        get { return rspButton; }
    }

    public List<ItemButtoninBattle> IBB
    {
        get { return ibb; }
        set { ibb = value; }
    }

    public int TURN
    {
        get { return turn; }
        set { turn = value; }
    }

    public int MYCHOICE
    {
        get { return myChoice; }
        set { myChoice = value; }
    }

    public int YOURCHOICE
    {
        get { return yourChoice; }
        set { yourChoice = value; }
    }

    public int MYCHOICE2
    {
        get { return myChoice2; }
        set { myChoice2 = value; }
    }

    public int YOURCHOICE2
    {
        get { return yourChoice2; }
        set { yourChoice2 = value; }
    }

    public string WATCH
    {
        get { return watch.text; }
        set { watch.text = value; }
    }

    public bool M1_Select
    {
        set
        { minus1_select.SetActive(value); }
    }

    BaseManager _BSM;
    battleJudgement _BTJ;
    resultWin _RSW;

    private void Awake()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();

        turn = 1;
        run = 1;
        ItemUsageStatus = 0;
        leftcount_array = new int[3] { 0, 0, 0 };

        tickets = new List<Sprite>();
        leftCount = new List<Text>();
        rspButton = new List<RSPbutton>();
        IBB = new List<ItemButtoninBattle>();
        usedPack = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            tickets.Add(ObjectPack.instance.SPRITEPACK["ticket_" + i]);
        }
        stamp = UIManager.instance.OpenWIn(Windows.stampWin).GetComponent<stampWin>();
        _RSW = UIManager.instance.MakeWindow(Windows.resultWin).GetComponent<resultWin>();
    }

    // Use this for initialization
    void Start()
    {
        _itemAction = new ItemAction();
        _BTJ = GetComponent<battleJudgement>();        

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(rspButtonPrefab, rsp_RL[0].transform);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0 + i * 200f, 0);
            rspButton.Add(go.GetComponent<RSPbutton>());
            leftCount.Add(go.GetComponentInChildren<Text>());
        }

        if (_BSM.INBATTLE.ISMINUS1)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject go = Instantiate(rspButtonPrefab, rsp_RL[1].transform);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0 + i * 200f, 0);
                rspButton.Add(go.GetComponent<RSPbutton>());
                leftCount.Add(go.GetComponentInChildren<Text>());
            }
        }

        for (int i = 0; i < rspButton.Count; i++)
            rspButton[i].init(i, this);

        init();
    }

    void Update()
    {
        if (ITEMACTION.ITEM4_LOADING)
        {
            submit.enabled = false;
            waitBar.fillAmount += Time.deltaTime * 6;
            waitBar.color = new Color(waitBar.fillAmount, 1f - waitBar.fillAmount, 0);
        }
    }

    void init()
    {
        myChoice = -1;
        YOURCHOICE = 0;
        myChoice2 = -1;
        YOURCHOICE2 = 0;

        myscore = 0;
        enemyscore = 0;
        submit.enabled = false;
        M1_S = minus1_select.GetComponent<Minus1_select>();
        minus1_select.SetActive(false);

        if (_BSM.INBATTLE.ISnoLIE || _BSM.INBATTLE.ISMINUS1)
            _lock.gameObject.SetActive(true);
    }

    void EndInit()
    {
        MYCHOICE = -1;
        MYCHOICE2 = -1;

        ITEMACTION.ITEM10 = false;
        ITEMACTION.ITEM10_Y = false;
        turn++;
        CheckItemEnable();
    }

    /// <summary>
    /// 가위바위보 버튼 비.활성화
    /// </summary>
    /// <param name="isStart"></param>
    void Setting_whenOnButton(bool isStart)
    {
        if (isStart)
        {
            // 비활성화
            for (int i = 0; i < 3; i++)
            {
                rspButton[i].closeButton();
            }
            for (int i = 0; i < IBB.Count; i++)
            {
                IBB[i].GetComponent<Button>().enabled = false;
            }
        }
        else
        {
            // 활성화
            for (int i = 0; i < 3; i++)
            {
                if (_BTJ.MYCOINpack[i] != 0)
                    rspButton[i].openButton();
            }
            CheckItemEnable();
        }
    }

    /// <summary>
    /// 제출 버튼
    /// </summary>
    public void Sumit_inBattle()
    {
        if (myChoice <= -1)
        {
            _BSM.ShowMessage("패를 고르자(혼잣말)");
            return;
        }

        Setting_whenOnButton(true);
        submit.enabled = false;
        percenTextIn("");
        _BTJ.suspect_superposition();

        // 상대 패고르기
        if (!_BSM.INBATTLE.ISMINUS1)
            _BTJ.RIVAL.Rivals_play();
        else
        {
            _BTJ.RIVAL.M1S_rivalSelect();
            _BTJ.RIVAL.rivalselect_2c();
        }

        ButtonETC_Reset();
        coroutin = OnButton();
        StartCoroutine(coroutin);
    }

    /// <summary>
    /// 버튼 선택시 이벤트(색변화, 개수 표시)
    /// </summary>
    public void EventToSelect()
    {
        submit.enabled = true;

        for (int i = 0; i < 3; i++)
        {
            leftcount_array[i] = _BTJ.MYCOINpack[i];
        }

        if (MYCHOICE != -1)
            leftcount_array[MYCHOICE]--;
        if (MYCHOICE2 != -1)
            leftcount_array[MYCHOICE2]--;

        for (int i = 0; i < 3; i++)
        {            
            leftCount[i].text = (leftcount_array[i]).ToString();

            if (_BSM.INBATTLE.ISMINUS1)
            {
                leftCount[i+3].text = (leftcount_array[i]).ToString();
            }
        }
        ButtonETC_Reset();
        if (myChoice != -1)
        {
            rspButton[myChoice].GetComponent<Image>().color = new Color(1.0f, 0.6f, 0.6f);
        }
        if (myChoice2 != -1)
        {
            rspButton[myChoice2+3].GetComponent<Image>().color = new Color(1.0f, 0.6f, 0.6f);
        }
    }

    /// <summary>
    /// 패 소지에 따른 버튼 온/오프
    /// </summary>
    public void ButtonETC_Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            if (leftcount_array[i] <= 0)
            {
                rspButton[i].GetComponent<Button>().enabled = false;
                rspButton[i].GetComponent<Image>().color = Color.gray;
                if (_BSM.INBATTLE.ISMINUS1)
                {
                    rspButton[i + 3].GetComponent<Button>().enabled = false;
                    rspButton[i + 3].GetComponent<Image>().color = Color.gray;
                }
            }
            else
            {
                rspButton[i].GetComponent<Button>().enabled = true;
                rspButton[i].GetComponent<Image>().color = new Color(1, 1, 1);
                if (_BSM.INBATTLE.ISMINUS1)
                {
                    rspButton[i+3].GetComponent<Button>().enabled = true;
                    rspButton[i+3].GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }
        }
    }

    // 한판 승패계산
    result battle()
    {
        if (ITEMACTION.ITEM10 && !ITEMACTION.ITEM10_Y)
            return result.win;
        else if(!ITEMACTION.ITEM10 && ITEMACTION.ITEM10_Y)
            return result.lose;

        int mValue = -1;
        int rValue = -1;
        if (!_BSM.INBATTLE.ISMINUS1)
        {
            mValue = myChoice;
            rValue = YOURCHOICE;
        }
        else
        {
            mValue = M1_S.SELECT;
            rValue = _BTJ.RIVAL.SELECT;
            Debug.Log(mValue+"::"+rValue);
        }

        if (mValue > rValue)
        {
            if (mValue == 2 && rValue == 0)
                return result.win;

            return result.lose;
        }
        else if (mValue == rValue)
            return result.drow;

        if (mValue == 0 && rValue == 2)
            return result.lose;
        return result.win;
    }

    // 아예 다끝났을때 승패계산
    result battle_FinalResult()
    {
        if (myscore > enemyscore)
            return result.win;            
        else if (myscore < enemyscore)
            return result.lose;

        return result.drow;
    }

    Image instaantiate_image(int meSelect, bool _IsSticker, bool my)
    {
        Image _image = Instantiate(ticket, stack.transform);
        _image.GetComponent<ticket>()._sticker(_IsSticker);
        usedPack.Add(_image.gameObject);

        int Rotate = Random.Range(0, 360);
        _image.sprite = tickets[meSelect];

        _image.rectTransform.anchoredPosition = new Vector2(0, ((my) ? -1200 : 1200));
        _image.rectTransform.rotation = Quaternion.Euler(0, 0, Rotate);

        return _image;
    }

    IEnumerator OnButton()
    {
        // 패 프리팹 생성
        Image myimage = instaantiate_image(myChoice, ITEMACTION.ITEM10, true); 
        Image yourimage = instaantiate_image(yourChoice, ITEMACTION.ITEM10, false);
        Image myimage2 = null, yourimage2 = null;

        if (_BSM.INBATTLE.ISMINUS1)
        {
            myimage2 = instaantiate_image(myChoice2, ITEMACTION.ITEM10, true);
            yourimage2 = instaantiate_image(yourChoice2, ITEMACTION.ITEM10, false);
        }

        // 이동(제출)
        if (!_BSM.INBATTLE.ISMINUS1)
        {
            iTween.MoveTo(myimage.gameObject, iTween.Hash("y", 810f, "time", 2.0f, "delay", 0.0f));
            iTween.MoveTo(yourimage.gameObject, iTween.Hash("y", 1510f, "time", 2.0f, "delay", 0.0f));
        }
        else
        {
            iTween.MoveTo(myimage.gameObject, iTween.Hash("x", 800f, "y", 810f, "time", 2.0f, "delay", 0.0f));
            iTween.MoveTo(yourimage.gameObject, iTween.Hash("x", 800f, "y", 1510f, "time", 2.0f, "delay", 0.0f));
            iTween.MoveTo(myimage2.gameObject, iTween.Hash("x", 440f, "y", 810f, "time", 2.0f, "delay", 0.0f));
            iTween.MoveTo(yourimage2.gameObject, iTween.Hash("x", 440f, "y", 1510f, "time", 2.0f, "delay", 0.0f));
        }

        // 손은 눈보다 빠르닷!!
        if (ITEMACTION.ITEM4)
        {
            _BSM.NOWUSER.PROBITY -= float.Parse(DataMng.instance.Get(LowDataType.ITEM).NODE["4"][(Item.Index.Cost).ToString()]) / ITEMACTION.ITEM4_VALUE;
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 0.05f;
            for (int i = 0; i < 3; i++)
            {
                rspButton[i].GetComponent<Button>().enabled = true;
            }

            waitBar.fillAmount = 0;
            ITEMACTION.ITEM4_LOADING = true;
            while (ITEMACTION.ITEM4_LOADING)
            {
                myimage.sprite = tickets[myChoice];
                myimage2.sprite = tickets[myChoice2];
                yield return new WaitForSeconds(0.01f);
                _BTJ.RIVAL.Scam_substitude();
                if (waitBar.fillAmount >= 1)
                {
                    ITEMACTION.ITEM4_LOADING = false;
                    break;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                rspButton[i].GetComponent<Button>().enabled = false;
            }
            waitBar.fillAmount = 0;
            Time.timeScale = 1;            
        }
        _BTJ.RIVAL.Scam_substitude();

        _BTJ.MYCOINpack[myChoice] -= 1;
        if (MYCHOICE2 != -1)
            _BTJ.MYCOINpack[MYCHOICE2] -= 1;

        ButtonETC_Reset();

        // 움직이면서 회전
        while (myimage.rectTransform.anchoredPosition.y < -250f) 
        {
            myimage.rectTransform.Rotate(new Vector3(0, 0, 120f * Time.deltaTime));
            yourimage.rectTransform.Rotate(new Vector3(0, 0, -120f * Time.deltaTime));
            if (_BSM.INBATTLE.ISMINUS1)
            {
                myimage2.rectTransform.Rotate(new Vector3(0, 0, 120f * Time.deltaTime));
                yourimage2.rectTransform.Rotate(new Vector3(0, 0, -120f * Time.deltaTime));
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1.0f);

        // 하나빼기 일때 하나빼기 대기
        if (_BSM.INBATTLE.ISMINUS1)
        {
            M1_Select = true;
            M1_S.SetImage(myimage, myimage2);
            Time.timeScale = 0;
            yield return new WaitForSeconds(0.1f);
            if(_BTJ.RIVAL.NUM == 0)
                yourimage2.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
            else if(_BTJ.RIVAL.NUM == 1)
                yourimage.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
            yield return new WaitForSeconds(0.9f);
        }        

        // 결과
        resultAndStamp();
        yield return new WaitForSeconds(2.0f);
        stamp.playAni("HIDE");

        //크기조절
        yield return new WaitForSeconds(0.2f);
        myimage.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
        yourimage.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;

        if (_BSM.INBATTLE.ISMINUS1)
        {
            myimage2.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
            yourimage2.GetComponent<RectTransform>().localScale = Vector3.one * 0.7f;
        }

        // 이동
        sideTable(myimage);
        sideTable(yourimage);
        if (_BSM.INBATTLE.ISMINUS1)
        {
            sideTable(myimage2);
            sideTable(yourimage2);
        }
                
        EndInit();

        if (!_BSM.INBATTLE.ISnoLIE && !_BSM.INBATTLE.ISMINUS1)
            _BTJ.Bot_Scolding();

        // 게임이 아직 안끝났으면 활성화 끝났으면 이제 활성화 할 필요가 없지
        if (!GameEnd())
            Setting_whenOnButton(false);
    }

    /// <summary>
    /// 게임 종료 여부 반환
    /// </summary>
    bool GameEnd()
    {
        if ((_BTJ.MYCOINpack[0] + _BTJ.MYCOINpack[1] + _BTJ.MYCOINpack[2] == 0) ||
            _BTJ.MYCOINpack[0] + _BTJ.MYCOINpack[1] + _BTJ.MYCOINpack[2] < Mathf.Abs(myscore-enemyscore))
        {
            StartCoroutine(takeTicket(battle_FinalResult()));
            return true;
        }
        return false;
    }

    // 승패 도장찍기
    void resultAndStamp()
    {
        result res = battle();
        switch (res)
        {
            case result.win:
                stamp.battleResult(res);
                myscore++;
                score[0].text = myscore.ToString();
                break;
            case result.lose:
                stamp.battleResult(res);
                enemyscore++;
                score[1].text = enemyscore.ToString();
                break;
            case result.drow:
                stamp.battleResult(res);
                break;
        }
    }


    /// <summary>
    /// 경기 끝나고 티켓 애니메이션대신 itween
    /// </summary>
    IEnumerator takeTicket(result res)
    {
        yield return new WaitForSeconds(2f);
        float x = 0, y = 0;        

        for (int i = 0; i < usedPack.Count; i++)
        {
            x = Random.Range(290f, 790f);
            y = Random.Range(910f, 1410f);
            iTween.MoveTo(usedPack[i].gameObject, iTween.Hash("x", x, "y", y, "time", 1.0f));
            yield return new WaitForSeconds(0.05f);
        }

        UIManager.instance.OpenWIn(Windows.resultWin);
        _RSW.init(battle_FinalResult());
        _RSW.playAni();
        yield return new WaitForSeconds(2f);

        switch (res)
        {
            case result.win:
                for (int i = 0; i < usedPack.Count; i++)
                {
                    iTween.MoveTo(usedPack[i].gameObject, iTween.Hash("x", 540f, "y", -140f, "time", 1.0f));
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case result.lose:
                for (int i = 0; i < usedPack.Count; i++)
                {
                    iTween.MoveTo(usedPack[i].gameObject, iTween.Hash("x", 540f, "y", 2160f, "time", 1.0f));
                    yield return new WaitForSeconds(0.05f);
                }
                break;
            case result.drow:
                int my = usedPack.Count / 2;
                for (int i = 0; i < usedPack.Count; i++)
                {
                    if (i < my)
                        iTween.MoveTo(usedPack[i].gameObject, iTween.Hash("x", 540f, "y", -140f, "time", 1.0f));
                    else
                        iTween.MoveTo(usedPack[i].gameObject, iTween.Hash("x", 540f, "y", 2160f, "time", 1.0f));
                    yield return new WaitForSeconds(0.05f);                    
                }                
                break;
        }
        _account(res);

        yield return new WaitForSeconds(1f);
        _BSM.DateCalculator();
        _BSM._loadLobbyScene("BattleScene");
    }

    // 게임끝나고 계산
    void _account(result res)
    {
        if (result.win == res)
        {
            foreach (int i in _BTJ.PACKED)
            {
                _BSM.NOWUSER.COIN[i] += _BSM.INBATTLE.MAGNIFY * run;
            }
        }
        else if (result.lose == res)
        {
            for (int i = 0; i < 3; i++)
            {
                if (_BSM.NOWUSER.COIN[i] >= (_BSM.INBATTLE.YOUR_BET[i] * _BSM.INBATTLE.MAGNIFY))
                    _BSM.NOWUSER.COIN[i] -= (_BSM.INBATTLE.YOUR_BET[i] * _BSM.INBATTLE.MAGNIFY * run);
                else
                {
                    int leftCoin = _BSM.NOWUSER.COIN[i];
                    int pay = _BSM.INBATTLE.YOUR_BET[i] * _BSM.INBATTLE.MAGNIFY - leftCoin;
                    _BSM.NOWUSER.COIN[i] = 0;
                    _BSM.NOWUSER.MONEY -= pay * (int)_BSM.NOWUSER.COINPRICE[i][4] * run;
                }
            }
        }

        if (_BSM.NOWUSER.MONEY < 0)
        {
            _BSM.NOWUSER.DEBT -= _BSM.NOWUSER.MONEY;
            _BSM.NOWUSER.MONEY = 0;
        }
    }

    // 뛰기
    public void Run()
    {
        if (myscore - enemyscore > 5)
        {
            myscore -= 5;
            _BSM.ShowMessage("뛰었다!!!!");
            run *= 5;
            score[0].text = myscore.ToString();
            GameObject go = Instantiate(mul5, mul5_parent);
            go.GetComponent<RectTransform>().position = new Vector3(730f, 1140f);
            go.GetComponentInChildren<Text>().text = "X" + run;
        }
    }

    /// <summary>
    /// 사용된 아이템 버튼 끄고켜기
    /// </summary>
    public void CheckItemEnable()
    {
        for (int i = 0; i < IBB.Count; i++)
        {
            IBB[i].CheckItemDuration();
            IBB[i].GetComponent<Button>().enabled = (IBB[i].ISUSED) ? false : true;            
        }
        if (ItemUsageStatus == 0) _BSM.NOWUSER.PROBITY += 0.1f;
        ItemUsageStatus = 0;
    }

    // 소매넣기 창
    public void underDeskWin(bool isOpen)
    {
        underDesk.SetActive(isOpen);
    }

    public void percenTextIn(string str1, string str2 = "", string str3 = "")
    {
        precenText[0].text = str1;
        precenText[1].text = str2;
        precenText[2].text = str3;
    }

    void sideTable(Image img)
    {
        float x = Random.Range(40f, 160f);
        float y = Random.Range(860f, 1460f);
        iTween.MoveTo(img.gameObject, iTween.Hash("x", x, "y", y, "time", 1.0f));
    }
}
