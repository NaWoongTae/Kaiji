using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inBattle{

    int[] limit;
    string stageName;

    int[] Bet_coin;
    int[] your_bet;
    int magnify;

    bool item3;
    int Iwant;

    bool item5;

    public bool ITEM3
    {
        get { return item3; }
        set { item3 = value; }
    }

    public int IWANT
    {
        get { return Iwant; }
        set { Iwant = value; }
    }

    public bool ITEM5
    {
        get { return item5; }
        set { item5 = value; }
    }

    public int[] LIMIT
    {
        get { return limit; }
        set { limit = value; }
    }
    public string STAGENAME
    {
        get { return stageName; }
        set { stageName = value; }
    }
    public int[] BET_COIN
    {
        get { return Bet_coin; }
        set { Bet_coin = value; }
    }

    public int[] YOUR_BET
    {
        get { return your_bet; }
        set { your_bet = value; }
    }

    public int MAGNIFY
    {
        get { return magnify; }
        set { magnify = value; }
    }

    public bool ISnoLIE
    {
        get
        {
            return STAGENAME.Equals("실화");
        }
    }

    public bool ISMINUS1
    {
        get
        {
            return STAGENAME.Equals("하나빼기");
        }
    }

    public inBattle() // 생성자로 초기화 가능?
    {
        limit = new int[2];
        stageName = "";
        Bet_coin = new int[3] { 0, 0, 0 };
        your_bet = new int[3] { 0, 0, 0 };
        magnify = 1;
        item3 = false;
        item5 = false;
    }	
}
