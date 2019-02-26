using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour {

    // 상점

    [SerializeField] Image img;
    Dictionary<string, Dictionary<string, string>> itemData = new Dictionary<string, Dictionary<string, string>>();
    BaseManager _BSM;
    LobbyManager _LBM;
    Text[] txt;
    int num = 0;
    
    private void Awake()
    {
        itemData = DataMng.instance.Get(LowDataType.ITEM).NODE;
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }
    
    public void init(int i)
    {
        num = i;
        txt = GetComponentsInChildren<Text>();
        if (num == 8 || num == 9) gameObject.GetComponent<Button>().enabled = false;
        Setting();
    }

    void Setting()
    {
        _LBM = ObjectPack.instance.Get(Managers.LobbyManager).GetComponent<LobbyManager>();
        img.sprite = ObjectPack.instance.SPRITEPACK["image_item_" + (num - 1)];
        
        txt[0].text = itemData[num.ToString()][(Item.Index.Name).ToString()];
        txt[1].text = itemData[num.ToString()][(Item.Index.Explan).ToString()];

        storeCount();
    }

    public void storeCount()
    {
        if (num == 8)
            txt[2].text = (_BSM.INNEWS.ITEM_COININFO) ? "있" : "없";
        else if (num == 9)
            txt[2].text = (_BSM.INNEWS.ITEM_BOSSINFO) ? "있" : "없";
        else
            txt[2].text = (_BSM.NOWUSER.ITEM[num - 1]).ToString();
    }

    public void BuyItem()
    {
        if (_BSM.NOWUSER.MONEY > int.Parse(itemData[num.ToString()][(Item.Index.Price).ToString()]))
        {
            _BSM.NOWUSER.MONEY -= int.Parse(itemData[num.ToString()][(Item.Index.Price).ToString()]);
            if (num != 8 && num != 9)
            {                
                _BSM.NOWUSER.ITEM[num - 1]++;
                txt[2].text = (_BSM.NOWUSER.ITEM[num - 1]).ToString();
                _LBM.transInit();
            }
            else
            {
                UIManager.instance.OpenWIn(Windows.NewsWin).GetComponent<NewsWin>().news(num);
            }
        }
        else
            _BSM.ShowMessage("돈이 없잖아 그지야");
    }
}
