using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour {

    BaseManager _BSM;
    Dictionary<string, Dictionary<string, string>> stageData = new Dictionary<string, Dictionary<string, string>>();
    Text[] txt;
    int num = 0;
    bool isBoss;

    public bool ISBOSS
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

    private void Awake()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        stageData = DataMng.instance.Get(LowDataType.STAGE).NODE;
        isBoss = false;
    }

    public void init(int i)
    {
        num = i;
        txt = GetComponentsInChildren<Text>();
        Setting();     
    }

    void Setting()
    {
        txt[0].text = stageData[num.ToString()][(Stage.Index.Name).ToString()];
        txt[1].text = stageData[num.ToString()][(Stage.Index.Explan).ToString()];
        txt[2].text = "[한도 : " + stageData[num.ToString()][(Stage.Index.downlimit).ToString()] + " ~ " 
            + stageData[num.ToString()][(Stage.Index.uplimit).ToString()] + "] [ " +
            stageData[num.ToString()][(Stage.Index.mul).ToString()] + "배 ]";
    }

    public void stage()
    {
        int dday = (int)_BSM.NOWUSER.DATE - _BSM.NOWUSER.BOSSDAY;
        if (!isBoss || ( dday >=0 && dday < 3 && _BSM.NOWUSER.BOSSDAY != 1))
        {
            _BSM.INBATTLE.LIMIT[0] = int.Parse(stageData[num.ToString()][Stage.Index.downlimit.ToString()]);
            _BSM.INBATTLE.LIMIT[1] = int.Parse(stageData[num.ToString()][Stage.Index.uplimit.ToString()]);
            _BSM.INBATTLE.STAGENAME = stageData[num.ToString()][Stage.Index.Name.ToString()];
            _BSM.INBATTLE.MAGNIFY = int.Parse(stageData[num.ToString()][Stage.Index.mul.ToString()]);
            UIManager.instance.OpenWIn(Windows.BattleRequestWin).GetComponent<BattleRequestWin>().openWin();
        }
        else
            _BSM.ShowMessage("회장님이 방문하셔야 싸울수있다.");
    }
}
