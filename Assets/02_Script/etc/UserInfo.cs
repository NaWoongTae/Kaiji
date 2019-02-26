using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    string _id;
    string _pw;
    
    int _money;
    int _debt;
    float _date;
    int _Whatday;

    float _probity;

    int[] _coin;
    List<float>[] _coinPrice;
    int[] _item;

    int coinday; // 날짜
    int cd_num; // 번호
    int cd_count; // 횟수 (최대 3)

    int bossday;
    int bd_num;

    float soundVolum;

    public string ID
    {
        get { return _id; }
        set { _id = value; }
    }
    public string PW
    {
        get { return _pw; }
        set { _pw = value; }
    }
    public int MONEY
    {
        get { return _money; }
        set { _money = value; }
    }
    public int DEBT
    {
        get { return _debt; }
        set { _debt = value; }
    }
    public float DATE
    {
        get { return _date; }
        set { _date = value; }
    }
    public int WHATDAY
    {
        get { return _Whatday; }
        set { _Whatday = value; }
    }
    public float PROBITY
    {
        get { return _probity; }
        set { _probity = value; }
    }
    public int[] COIN
    {
        get { return _coin; }
        set { _coin = value; }
    }
    public List<float>[] COINPRICE
    {
        get { return _coinPrice; }
        set { _coinPrice = value; }
    }
    public int[] ITEM
    {
        get { return _item; }
        set { _item = value; }
    }

    public int COINDAY
    {
        get { return coinday; }
        set { coinday = value; }
    }

    public int CD_NUM
    {
        get { return cd_num; }
        set { cd_num = value; }
    }

    public int CD_COUNT
    {
        get { return cd_count; }
        set { cd_count = value; }
    }

    public int BOSSDAY
    {
        get { return bossday; }
        set { bossday = value; }
    }

    public int BD_NUM
    {
        get { return bd_num; }
        set { bd_num = value; }
    }

    public float SOUNDVOLUM
    {
        get { return soundVolum; }
        set { soundVolum = value; }
    }

    public UserInfo()
    {
        _id = string.Empty;
        _pw = string.Empty;
        _money = 0;
        _debt = 0;        
        _date = 0f;
        _Whatday = 0;
        _probity = 0;
        _coin = new int[3] { 0, 0, 0 };
        _coinPrice = new List<float>[3] { new List<float> { 0, 0, 0, 0, 0 }, new List<float> { 0, 0, 0, 0, 0 }, new List<float> { 0, 0, 0, 0, 0 } };
        _item = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        coinday = 1;
        cd_num = 0;
        cd_count = 0;
        bossday = 1;
        bd_num = 0;
        soundVolum = 0.5f;
    }

    public UserInfo(string id, string pw)
    {
        _id= id;
        _pw = pw;
        _money = 30000;
        _debt = 100000000;
        _date = 1f;
        _Whatday = Random.Range(1,7);
        _probity = 100; 
        _coin = new int[3] { 10, 10, 10 };
        _coinPrice = new List<float>[3] { new List<float> { 600f, 700f, 800f, 900f, 1000f },
            new List<float> { 600f, 700f, 800f, 900f, 1000f  }, new List<float> { 600f, 700f, 800f, 900f, 1000f } };
        _item = new int[10] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5};
        coinday = 1;
        cd_num = 0;
        cd_count = 0;
        bossday = 1;
        bd_num = 0;
        soundVolum = 0.5f;
    }

    public UserInfo(string id, string pw, int i)
    {
        _id = id;
        _pw = pw;
        _money = 30000000;
        _debt = 100000000;
        _date = 1f;
        _Whatday = Random.Range(1, 7);
        _probity = 100;
        _coin = new int[3] { 300, 300, 300 };
        _coinPrice = new List<float>[3] { new List<float> { 600f, 700f, 800f, 900f, 1200f },
            new List<float> { 600f, 700f, 800f, 900f, 1200f  }, new List<float> { 600f, 700f, 800f, 900f, 1200f } };
        _item = new int[10] { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99 };
        coinday = 1;
        cd_num = 0;
        cd_count = 0;
        bossday = 1;
        bd_num = 0;
        soundVolum = 0.5f;
    }
}
