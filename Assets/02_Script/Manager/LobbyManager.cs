using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

    // 0 : 아이디&랭크 1: 일차 2 : 빚 3: 자본 4: 바위코인 5 : 가위코인 6 : 보코인
    [SerializeField] Text[] playerinfoDisplay;
    GameObject money;

    BaseManager _BSM;
    CoinWin _COW;
    OptionWin _OPW;
    MenuWin _MNW;    

    void Awake()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        
                
        init_Win();
        
    }

    // Use this for initialization
    void Start()
    {
        init();
        money = playerinfoDisplay[3].GetComponentInParent<Image>().gameObject;
        _BSM.SaveUserData();
        //day_debug();
    }

    void day_debug()
    {
        Debug.Log("-------------------------");
        Debug.Log("COINDAY : " + _BSM.NOWUSER.COINDAY + " - CD_NUM : " + _BSM.NOWUSER.CD_NUM + " - CD_COUNT : " + _BSM.NOWUSER.CD_COUNT);
        Debug.Log("BOSSDAY : " + _BSM.NOWUSER.BOSSDAY + " - BD_NUM : " + _BSM.NOWUSER.BD_NUM);
    }

	// Update is called once per frame
	void Update () {
		
	}

    void init()
    {
        if (_BSM.NOWUSER.PROBITY > 100.0f)
            _BSM.NOWUSER.PROBITY = 100.0f;

        playerinfoDisplay[0].text = _BSM.NOWUSER.ID + " / " + string.Format("{0:N1}", _BSM.NOWUSER.PROBITY);
        if (!_BSM.TodayIsWeekend())
            playerinfoDisplay[1].text = ((int)_BSM.NOWUSER.DATE).ToString() + "일차";
        else
            playerinfoDisplay[1].text = "<b><color=red>" + ((int)_BSM.NOWUSER.DATE).ToString() + "일차</color></b>";
        playerinfoDisplay[2].text = "빚 : " + _BSM.NOWUSER.DEBT.ToString();
        playerinfoDisplay[3].text = "자본 : " + _BSM.NOWUSER.MONEY.ToString();
        playerinfoDisplay[4].text = "바위 : " + _BSM.NOWUSER.COIN[0].ToString();
        playerinfoDisplay[5].text = "가위 : " + _BSM.NOWUSER.COIN[1].ToString(); 
        playerinfoDisplay[6].text = "보 : " + _BSM.NOWUSER.COIN[2].ToString();
        _MNW.GetComponentInChildren<StoreinMenu>().open_Info((int)_BSM.NOWUSER.DATE);        
    }

    public void transInit()
    {
        playerinfoDisplay[3].text = "자본 : " + _BSM.NOWUSER.MONEY.ToString();
        playerinfoDisplay[4].text = "바위 : " + _BSM.NOWUSER.COIN[0].ToString();
        playerinfoDisplay[5].text = "가위 : " + _BSM.NOWUSER.COIN[1].ToString();
        playerinfoDisplay[6].text = "보 : " + _BSM.NOWUSER.COIN[2].ToString();

        _MNW.GetComponentInChildren<StoreinMenu>().open_Info((int)_BSM.NOWUSER.DATE);
        _BSM.SaveUserData();
    }

    void init_Win()
    {
        _OPW = UIManager.instance.MakeWindow(Windows.OptionWin).GetComponent<OptionWin>();
        _COW = UIManager.instance.MakeWindow(Windows.CoinWin).GetComponent<CoinWin>();
        _MNW = UIManager.instance.OpenWIn(Windows.MenuWin).GetComponent<MenuWin>();
    }

    public void optionButton()
    {
        _OPW.openOption();
    }

    public void Rcoin()
    {
        UIManager.instance.OpenWIn(Windows.CoinWin).GetComponent<CoinWin>().Graph();
        _COW.whenOpeninit(CoinWin.RSP.바위);
    }
    public void Scoin()
    {
        UIManager.instance.OpenWIn(Windows.CoinWin).GetComponent<CoinWin>().Graph();
        _COW.whenOpeninit(CoinWin.RSP.가위);        
    }
    public void Pcoin()
    {
        UIManager.instance.OpenWIn(Windows.CoinWin).GetComponent<CoinWin>().Graph();
        _COW.whenOpeninit(CoinWin.RSP.보);
    }

    public void moveMoneyWin(bool bl)
    {
        if(!bl)
            iTween.MoveTo(money, iTween.Hash("y", 131f, "time", 1.0f));
        else
            iTween.MoveTo(money, iTween.Hash("y", 281f, "time", 1.0f));
    }

    public void myTotalMoney()
    {
        int myMoney = _BSM.NOWUSER.MONEY;
        for (int i = 0; i < 3; i++)
        {
            myMoney += _BSM.NOWUSER.COIN[i] * (int)_BSM.NOWUSER.COINPRICE[i][4];
        }
        _BSM.ShowMessage("총자본 : " + myMoney.ToString());
    }
}
