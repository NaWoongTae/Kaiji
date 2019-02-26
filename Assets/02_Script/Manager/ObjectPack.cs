using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Managers
{
    BaseManager,
    LogoManager,
    LoginManager,
    LobbyManager,
    BattleManager,
    WorkManager,

    DataMng,
    UIManager,

    Max
}

public class ObjectPack : TSingleton<ObjectPack> {

    DataMng _DTM;
    UIManager _UIM;

    BaseManager _BSM;

    LogoManager _LOM;    
    loginManager _LGM;
    LobbyManager _LBM;    
    BattleManager _BTM;

    Dictionary<string, Sprite> spritePack;

    public Dictionary<string, Sprite> SPRITEPACK
    {
        get { return spritePack; }
    }

    public Dictionary<Managers, GameObject> MngDic = new Dictionary<Managers, GameObject>();
    Dictionary<Managers, GameObject> MngPrefabDic = new Dictionary<Managers, GameObject>();

    // Use this for initialization
    void Start ()
    {
        MngDic.Add(Managers.DataMng, DataMng.instance.gameObject);
        MngDic.Add(Managers.UIManager, UIManager.instance.gameObject);
    }



    protected override void Init()
    {
        base.Init();

        for (int i = (int)Managers.LogoManager; i < (int)Managers.Max; i++)
        {
            GameObject prefab = Resources.Load("Prefabs\\Managers\\" + ((Managers)i).ToString()) as GameObject;
            MngPrefabDic.Add((Managers)i, prefab);
        }

        getinSprite();
    }

    public GameObject MakeManager(Managers mng)
    {
        if (MngDic.ContainsKey(mng))
            MngDic.Remove(mng);        

        GameObject go = Instantiate(MngPrefabDic[mng]);
        MngDic.Add(mng, go);

        return MngDic[mng];
    }

    public void MakeManager(int number)
    {
        MakeManager((Managers)number);
    }

    public GameObject Get(Managers mng)
    {
        if (!MngDic.ContainsKey(mng))
            return null;

        return MngDic[mng];
    }

    //----------------------------------------------------
    void getinSprite()
    {
        spritePack = new Dictionary<string, Sprite>();
        for (int i = 0; i < 4; i++)
        {
            spritePack.Add("ticket_" + i, Resources.Load<Sprite>("Sprite/ticket_" + i));
        }
        spritePack.Add("winStamp", Resources.Load<Sprite>("Sprite/winStamp"));
        spritePack.Add("drowStamp", Resources.Load<Sprite>("Sprite/drowStamp"));
        for (int i = 0; i < 3; i++)
        {
            spritePack.Add("rsp_" + i, Resources.Load<Sprite>("Sprite/rsp_" + i));
        }
        for (int i = 1; i <= 6; i++)
        {
            spritePack.Add("result_" + i, Resources.Load<Sprite>("Sprite/result_" + i));
        }
        for (int i = 0; i < 10; i++)
        {
            spritePack.Add("image_item_" + i, Resources.Load<Sprite>("Sprite/image_item_" + i));
        }
    }
}
