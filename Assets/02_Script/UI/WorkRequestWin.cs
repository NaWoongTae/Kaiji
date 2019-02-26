using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkRequestWin : MonoBehaviour {

    

    [SerializeField] InputField input;
    [SerializeField] Text txt;

    BaseManager _BSM;

	// Use this for initialization
	public void init(inWork.work work)
    {
        txt.text = work.ToString() + "작업 몇일가능 합니까?";        
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _BSM.INWORK.WORK = work;
    }

    public void okButton()
    {        
        try
        {
            int day = int.Parse(input.text);
            int isWeekend = _BSM.WorkDateCalculator(day);

            if (isWeekend == 0)
            {
                _BSM.INWORK.DAY = day;
                UIManager.instance.MakeWindow(Windows.MenuWin).GetComponent<MenuWin>().OnCloseStore();
                ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>()._loadWorkScene("LobbyScene");
            }
            else
            {
                _BSM.ShowMessage(isWeekend + "일 후는 주말입니다.");
            }
        }
        catch
        {
            _BSM.ShowMessage("숫자만 넣어라..");
        }        
    }

    public void noButton()
    {
        gameObject.SetActive(false);
    }
}
