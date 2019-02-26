using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleRequestWin : MonoBehaviour {

    [SerializeField] Button itemButton;

    [SerializeField] InputField[] _coins;
    [SerializeField] Text _total;
    [SerializeField] Text _limitText;
    [SerializeField] Text _stageName;

    int[] limit;
    int totalAmount;
    BaseManager _BSM;

    private void Awake()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        init();

        itemButton.GetComponentInChildren<Text>().text = DataMng.instance.Get(LowDataType.ITEM).NODE["3"]["Name"];
    }

    void LateUpdate()
    {
        inputCheck();
        _isTotal();
    }

    void init()
    {
        _stageName.text = _BSM.INBATTLE.STAGENAME;
        totalAmount = 0;
        limit = _BSM.INBATTLE.LIMIT;
        foreach (InputField iField in _coins)
        {
            iField.text = "0";
        }
        _limitText.text = "한도 : " + limit[0].ToString() + " - " + limit[1].ToString();
        _total.text = totalAmount.ToString();

        _BSM.INBATTLE.ITEM3 = false;

        if (_BSM.INBATTLE.ISnoLIE || _BSM.INBATTLE.ISMINUS1)
        {
            itemButton.enabled = false;
            itemButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            itemButton.enabled = true;
            itemButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void init2()
    {
        _stageName.text = _BSM.INBATTLE.STAGENAME;
        limit = _BSM.INBATTLE.LIMIT;
        _limitText.text = "한도 : " + limit[0].ToString() + " - " + limit[1].ToString();
    }

    public void R_up()
    {
        if (_BSM.NOWUSER.COIN[0] - int.Parse(_coins[0].text) > 0)
        {
            _coins[0].text = (int.Parse(_coins[0].text) + 1).ToString();
        }
        else
            _BSM.ShowMessage("코인이 더 없잖아!!");
    }

    public void R_down()
    {
        int i = int.Parse(_coins[0].text);
        if (i > 0)
            i--;
        _coins[0].text = i.ToString();
    }

    public void S_up()
    {
        if (_BSM.NOWUSER.COIN[1] - int.Parse(_coins[1].text) > 0)
        {
            _coins[1].text = (int.Parse(_coins[1].text) + 1).ToString();
        }
        else
            _BSM.ShowMessage("코인이 더 없잖아!!");
    }

    public void S_down()
    {
        int i = int.Parse(_coins[1].text);
        if (i > 0)
            i--;
        _coins[1].text = i.ToString();
    }

    public void P_up()
    {
        if (_BSM.NOWUSER.COIN[2] - int.Parse(_coins[2].text) > 0)
        {
            _coins[2].text = (int.Parse(_coins[2].text) + 1).ToString();
        }
        else
            _BSM.ShowMessage("코인이 더 없잖아!!");
    }

    public void P_down()
    {
        int i = int.Parse(_coins[2].text);
        if (i > 0)
            i--;
        _coins[2].text = i.ToString();
    }

    // inputfield로 들어오는 값이
    // 코인개수 이내인지 체크
    void inputCheck()
    {
        try
        {
            for (int i = 0; i < 3; i++)
            {
                if (int.Parse(_coins[i].text) > _BSM.NOWUSER.COIN[i])
                {
                    _coins[i].text = _BSM.NOWUSER.COIN[i].ToString();
                    _BSM.ShowMessage("어딜 없는 돈을 걸어~?");
                }
            }
        }
        catch
        {
            _BSM.ShowMessage("숫자만 쓰라고~~~");
        }
    }

    void _isTotal()
    {
        _stageName.text = _BSM.INBATTLE.STAGENAME;
        _limitText.text = "한도 : " + limit[0].ToString() + " - " + limit[1].ToString();
        float tot = 0;
        for (int i = 0; i < 3; i++)
        {
            tot += _BSM.PRICE[i] * float.Parse(_coins[i].text);
        }
        totalAmount = (int)tot;
        _total.text = "총 베팅액 : " + totalAmount.ToString();
    }

    public void openWin()
    {
        init();
    }

    public void goPlay()
    {
        if (limit[0] <= totalAmount && limit[1] >= totalAmount)
        {            
            for (int i = 0; i < 3; i++)
                _BSM.INBATTLE.BET_COIN[i] = int.Parse(_coins[i].text);            
            init2();
            UIManager.instance.MakeWindow(Windows.MenuWin).GetComponent<MenuWin>().OnCloseStore();
            _BSM._loadBattleScene("LobbyScene");
        }
        else
            _BSM.ShowMessage(limit[0].ToString() + " - " + limit[1].ToString() + " 안에서 베팅하라구~");

    }

    public void cancel()
    {
        init();
        gameObject.SetActive(false);
    }

    public void aboutItemButton()
    {
        if (_BSM.NOWUSER.ITEM[2] == 0)
        {
            itemButton.enabled = false;
            itemButton.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            itemButton.enabled = true;
            itemButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void button_3()
    {
        if (!_BSM.INBATTLE.ITEM3)
        {
            _BSM.INBATTLE.ITEM3 = true;
            itemButton.GetComponent<Image>().color = Color.yellow;
            UIManager.instance.OpenWIn(Windows.item3_RequestWin).GetComponent<item3_RequestWin>().openWin();            
        }
        else
        {
            button3_cancle();
        }
    }

    public void button3_cancle()
    {
        _BSM.INBATTLE.ITEM3 = false;
        itemButton.GetComponent<Image>().color = Color.white;
    }    
}
