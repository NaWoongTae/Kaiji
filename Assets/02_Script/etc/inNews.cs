using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inNews{

    bool item_Coininfo;
    bool item_Bossinfo;    
    
    int coin_kind;
    bool updown;    

    bool boss_fight;

    public bool ITEM_COININFO
    {
        get { return item_Coininfo; }
        set { item_Coininfo = value; }
    }
    public bool ITEM_BOSSINFO
    {
        get { return item_Bossinfo; }
        set { item_Bossinfo = value; }
    }
    
    public int COIN_KIND
    {
        get { return coin_kind; }
        set { coin_kind = value; }
    }
    
    public bool UPDOWN
    {
        get { return updown; }
        set { updown = value; }
    }
    
    public bool BOSS_FIGHT
    {
        get { return boss_fight; }
        set { boss_fight = value; }
    }

    public inNews()
    {
        coin_kind = 4;
        item_Coininfo = false;
        item_Bossinfo = false;
        boss_fight = false;
    }
}
