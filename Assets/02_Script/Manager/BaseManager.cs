using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BaseManager : MonoBehaviour {    

    Dictionary<string, UserInfo> _userinfo;
    UserInfo _nowuser;
    
    XmlClass XC;
    MessageWin _MSW;

    List<float>[] PriceRecord;
    int[] Price;
    float time;
        
    // in battle
    inBattle _inBattle;	
    inWork _inWork;
    inNews _inNews;
        
    public float TIME_coin
    {
        get { return time; }
        set { time = value; }
    }

    public inBattle INBATTLE
    {
        get { return _inBattle; }
        set { _inBattle = value; }
    }

    public inWork INWORK
    {
        get { return _inWork; }
        set { _inWork = value; }
    }

    public inNews INNEWS
    {
        get { return _inNews; }
        set { _inNews = value; }
    }

    public Dictionary<string, UserInfo> USERINFO
    {
        get
        {
            return _userinfo;
        }
        set
        {
            _userinfo = value;
        }
    }
    public UserInfo NOWUSER
    {
        get
        {
            return _nowuser;
        }
        set
        {
            _nowuser = value;
        }
    }
    public int[] PRICE
    {
        get
        {
            return Price;
        }
        set
        {
            Price = value;
        }
    }

    public List<float>[] PRICE_RECORD
    {
        get
        {
            return PriceRecord;
        }
        set
        {
            PriceRecord = value;
        }
    }

    

    void Awake()
    {    
        XC = new XmlClass();
        _userinfo = new Dictionary<string, UserInfo>();
        _nowuser = new UserInfo();
        _inBattle = new inBattle();
        _inWork = new inWork();
        _inNews = new inNews();

        Price = new int[3];
        PriceRecord = new List<float>[3] { new List<float> { 0, 0, 0, 0, 0 }, new List<float> { 0, 0, 0, 0, 0 }, new List<float> { 0, 0, 0, 0, 0 } };
    }

    // Use this for initialization
    void Start()
    {
        ObjectPack.instance.MngDic.Add(Managers.BaseManager, gameObject);
        _MSW = UIManager.instance.MakeWindow(Windows.MessageWin).GetComponent<MessageWin>();

        if (File.Exists(XC.FILENAME))
        {
            USERINFO = XC.XmlLoad();
            //foreach (KeyValuePair<string, UserInfo> us in XC.XmlLoad())
            //{
            //    USERINFO.Add(us.Key, us.Value);
            //}
        }
        SaveUserData();

        StartCoroutine(StartLogoScene());
        DataMng.instance.LoadData();
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        if (TIME_coin > 90f)
        {
            CoinExchange();
            TIME_coin = 0f;
        }
    }

    public void SaveUserData()
    {
        XC.XmlSave(USERINFO);
    }

    IEnumerator LoadingScene(string remove, int load)
    {
        UIManager.instance.AllClose();
        UIManager.instance.OpenWIn(Windows.LoadingWin);

        AsyncOperation AO;
        if (remove != string.Empty)
        {
            AO = SceneManager.UnloadSceneAsync(remove);
            while (!AO.isDone)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        yield return new WaitForSeconds(0.5f);

        AO = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive); ;
        while (!AO.isDone)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        yield return new WaitForSeconds(0.2f);
        ObjectPack.instance.MakeManager(load);

        UIManager.instance.closeWin(Windows.LoadingWin);
    }

    IEnumerator StartLogoScene()
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync("LogoScene", LoadSceneMode.Additive);
        while (!AO.isDone)
        {
            yield return new WaitForSeconds(0.3f);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        ObjectPack.instance.MakeManager(Managers.LogoManager);
    }
    
    public void _loadloginScene(string remove = "")
    {
        StartCoroutine(LoadingScene(remove, 2));
        SoundManager.instance.playBGMsound(SoundManager.TYPEBGM.LOGIN, 0.5f);
    }

    public void _loadLobbyScene(string remove = "")
    {
        StartCoroutine(LoadingScene(remove, 3));
        SoundManager.instance.playBGMsound(SoundManager.TYPEBGM.LOBBY, NOWUSER.SOUNDVOLUM);
    }

    public void _loadBattleScene(string remove = "")
    {
        StartCoroutine(LoadingScene(remove, 4));
        SoundManager.instance.playBGMsound(SoundManager.TYPEBGM.BATTLE, NOWUSER.SOUNDVOLUM);
    }

    public void _loadWorkScene(string remove = "")
    {
        StartCoroutine(LoadingScene(remove, 5));
        SoundManager.instance.playBGMsound(SoundManager.TYPEBGM.WORK, NOWUSER.SOUNDVOLUM);
    }
    
    public void ShowMessage(string str)
    {
        _MSW.OnMessage(str);
        SoundManager.instance.playEFFECTsound(SoundManager.TYPEEFFECT.WINDOW, NOWUSER.SOUNDVOLUM, false);
        UIManager.instance.OpenWIn(Windows.MessageWin);
    }

    public bool TodayIsWeekend()
    {
        if (((int)NOWUSER.DATE + NOWUSER.WHATDAY) % 7 == 1)
            return true;
        else
            return false;
    }

    public void DateCalculator()
    {
        if (((int)NOWUSER.DATE + NOWUSER.WHATDAY) % 7 != 1)
            NOWUSER.DATE += 0.5f;
        else
            NOWUSER.DATE += 0.35f;
    }

    public int WorkDateCalculator(int day)
    {
        for (int i = 0; i < day; i++)
        {
            if (((int)NOWUSER.DATE + NOWUSER.WHATDAY + i) % 7 == 1)
            {
                return i;
            }
        }
        return 0;
    }

    public void CoinExchange()
    {
        if (NOWUSER != null)
        {
            for (int i = 0; i < PRICE_RECORD.Length; i++)
            {
                for (int j = 0; j < PRICE_RECORD[i].Count - 1; j++)
                {
                    PRICE_RECORD[i][j] = PRICE_RECORD[i][j + 1];
                }
                              
                if(item_coininfo(i))
                    PRICE_RECORD[i][4] = PRICE_RECORD[i][3] * UnityEngine.Random.Range(0.8f, 1.2f);                

                PRICE[i] = (int)(PRICE_RECORD[i][4]);
            }
            SaveUserData();
        }
        if (UIManager.instance.isOpen(Windows.CoinWin))
            UIManager.instance.MakeWindow(Windows.CoinWin).GetComponent<CoinWin>().Graph();
    }

    bool item_coininfo(int i)
    {
        if ((NOWUSER.CD_COUNT != 0) && ((i == INNEWS.COIN_KIND) || INNEWS.COIN_KIND == 3))
        {
            int R = UnityEngine.Random.Range(0, 101);

            float value;
            if (R <= 30 + 20 * NOWUSER.CD_COUNT)
                value = isUp_inItem(INNEWS.UPDOWN);
            else
                value = isUp_inItem(!INNEWS.UPDOWN);

            PRICE_RECORD[i][4] = PRICE_RECORD[i][3] * value;
            NOWUSER.CD_COUNT--;

            if (NOWUSER.CD_COUNT == 0)
                INNEWS.ITEM_COININFO = false;
            return false;
        }
        else
            return true;
    }

    public float isUp_inItem(bool bl)
    {
        if(bl)
            return UnityEngine.Random.Range(1.1f, 1.4f);
        else
            return UnityEngine.Random.Range(0.7f, 1.0f);
    }
}
