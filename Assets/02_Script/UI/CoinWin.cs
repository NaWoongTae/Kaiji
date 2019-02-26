using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinWin : MonoBehaviour {

    //struct SettingRequest
    //{
    //    public Action[] _callback;

    //    public SettingRequest(Action[] callback)
    //    {
    //        _callback = callback;
    //    }
    //}

    public enum RSP { 바위, 가위, 보};

    [SerializeField] Text _coinName;
    [SerializeField] InputField _number;
    [SerializeField] Text _total;
    [Space]
    [SerializeField] GameObject linePrefabs;
    [SerializeField] Transform GrapfParents;

    //SettingRequest _settingRequest;
    int whatRSP = 0;
    int _price;

    int _totalPrice;
    int _numberOfTrade;
    BaseManager _BSM;
    LobbyManager _LBM;

    List<DrawLine> GraphList;
    float GrapfMaxPos = 1385f;
    float GrapfMinPos = 835f;

	// Use this for initialization
	void Awake ()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        // _settingRequest = new SettingRequest(new Action[3] { is_R, is_S, is_P });
        _number.text = "1";
        gameObject.SetActive(false);

        GraphList = new List<DrawLine>();
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(linePrefabs, GrapfParents);
            GraphList.Add(go.GetComponent<DrawLine>());
        }
    }

    void Start()
    {
        _LBM = ObjectPack.instance.Get(Managers.LobbyManager).GetComponent<LobbyManager>();
        // Graph();
    }

    void Update()
    {
        calculator();        
    }

    public void whenOpeninit(RSP _RSP)
    {
        whatRSP = (int)_RSP;
        //_settingRequest._callback[whatRSP]();
        is_RSP();       
    }

    public void nextButton()
    {
        if (whatRSP != 2)
            whatRSP++;
        else
            whatRSP = 0;
        
        is_RSP();
        //_settingRequest._callback[whatRSP]();
    }

    public void prevButton()
    {
        if (whatRSP != 0)
            whatRSP--;
        else
            whatRSP = 2;
        
        is_RSP();
        //_settingRequest._callback[whatRSP]();
    }
    
    void is_RSP()
    {
        _coinName.text = (RSP)whatRSP + " 배급표 시세";
        _price = (int)_BSM.PRICE[whatRSP];
        Graph();
    }

    void calculator()
    {
        try
        {
            if (!_number.text.Equals(string.Empty))
                _numberOfTrade = int.Parse(_number.text);
            else
                _numberOfTrade = 0;

            _price = (int)_BSM.PRICE[whatRSP];
            int Sum = _numberOfTrade * _price;

            _totalPrice = Sum;
            _total.text = _totalPrice.ToString();
        }
        catch
        {
            _BSM.ShowMessage("숫자만 쓰시라고~~");
        }
    }

    public void Buy()
    {
        _BSM.NOWUSER.MONEY -= _totalPrice;
        _BSM.NOWUSER.COIN[whatRSP] += _numberOfTrade;
        _number.text = "1";
        _LBM.transInit();
    }

    public void Sell()
    {
        _BSM.NOWUSER.MONEY += _totalPrice;
        _BSM.NOWUSER.COIN[whatRSP] -= _numberOfTrade;
        _number.text = "1";
        _LBM.transInit();
    }

    public void Cancel()
    {
        gameObject.SetActive(false);        
    }

    public void Graph()
    {        
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        float Maxprice = _BSM.PRICE_RECORD[whatRSP][0];
        float Minprice = _BSM.PRICE_RECORD[whatRSP][0];

        for (int i = 0; i < _BSM.PRICE_RECORD[whatRSP].Count; i++)
        {
            if (Maxprice < _BSM.PRICE_RECORD[whatRSP][i])
                Maxprice = _BSM.PRICE_RECORD[whatRSP][i];
            else if (Minprice > _BSM.PRICE_RECORD[whatRSP][i])
                Minprice = _BSM.PRICE_RECORD[whatRSP][i];
        }
        List<float> percentList = new List<float>();
        
        for (int i = 0; i < _BSM.PRICE_RECORD[whatRSP].Count; i++)
        {
            float Hpercent = (_BSM.PRICE_RECORD[whatRSP][i] - Minprice) / (Maxprice - Minprice);
            percentList.Add(Hpercent);
        }        

        Vector2 start;
        Vector2 end = new Vector2();
        
        for (int i = 0; i < _BSM.PRICE_RECORD[whatRSP].Count-1; i++)
        {
            float startpos = (GrapfMaxPos - GrapfMinPos) * percentList[i] + GrapfMinPos;
            float endpos = (GrapfMaxPos - GrapfMinPos) * percentList[i+1] + GrapfMinPos;
            start = new Vector2(110f + i * 215f, startpos);
            end = new Vector2(325f + i * 215, endpos);
            GraphList[i].init(start, end);
        }
    }
}
