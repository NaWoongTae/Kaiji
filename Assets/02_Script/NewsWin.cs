using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsWin : MonoBehaviour {

    [SerializeField] Text txt;
    [SerializeField] Text updown;

    Dictionary<string, Dictionary<string, string>> newsData;
    BaseManager _BSM;        
    
    private void Awake()
    {
        newsData = new Dictionary<string, Dictionary<string, string>>();
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
    }

    // Use this for initialization
    public void news(int num)
    {
        switch (num)
        {
            case 8:
                num_8();
                break;
            case 9:
                num_9();
                break;
        }

    }

    void num_8()
    {
        newsData = DataMng.instance.Get(LowDataType.COININFO).NODE;

        if (_BSM.INNEWS.ITEM_COININFO && _BSM.NOWUSER.CD_COUNT == 0)
        {
            _BSM.NOWUSER.CD_NUM = Random.Range(0, newsData.Count) + 1;            
            _BSM.NOWUSER.CD_COUNT = 3;
            _BSM.INNEWS.UPDOWN = (int.Parse(newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.Fluctuation.ToString()]) == 1);
            if (int.Parse(newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.ConnectCount.ToString()]) == 1)
                _BSM.INNEWS.COIN_KIND = Random.Range(0, 3);
            else
                _BSM.INNEWS.COIN_KIND = 3;
        }
        string ex1 = newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.Explan.ToString()];
        string ex2 = newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.ExplanAdd.ToString()];

        if (ex1.Equals("null"))
            ex1 = "";
        if (ex2.Equals("null"))
            ex2 = "";

        string rsp = "";

        if (int.Parse(newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.ConnectCount.ToString()]) == 1)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    rsp = " '바위'";
                    break;
                case 1:
                    rsp = " '가위'";
                    break;
                case 2:
                    rsp = " '보'";
                    break;
            }
        }
        else
            rsp = "";

        txt.text = ex1 + rsp + ex2;
        updown.text = (int.Parse(newsData[_BSM.NOWUSER.CD_NUM.ToString()][Coininfo.Index.Fluctuation.ToString()]) == 1) ? "떡상!?" : "떡락?!";
    }

    void num_9()
    {
        newsData = DataMng.instance.Get(LowDataType.BOSSINFO).NODE;

        if (_BSM.INNEWS.ITEM_BOSSINFO && _BSM.NOWUSER.BD_NUM == 0)
        {
            _BSM.NOWUSER.BD_NUM = Random.Range(0, newsData.Count) + 1;
            return;
        }
        txt.text = newsData[_BSM.NOWUSER.BD_NUM.ToString()][Bossinfo.Index.Explan.ToString()];
        updown.text = _BSM.NOWUSER.BOSSDAY + "일 예정";
    }

	// Update is called once per frame
	public void cancel ()
    {
        gameObject.SetActive(false);	
	}
}
