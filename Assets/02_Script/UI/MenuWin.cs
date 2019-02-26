using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWin : MonoBehaviour {

    [SerializeField] GameObject _Store;
    [SerializeField] GameObject _escBackBoard;
    [SerializeField] GameObject[] _taps;

    bool isOpen;
    BaseManager _BSM;

    void Awake()
    {
        isOpen = false;
        _escBackBoard.SetActive(false);
    }

	// Use this for initialization
	void Start ()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
    }

    public void OnStoreButton()
    {
        buttonCommonEvent();        
        _taps[0].transform.SetSiblingIndex(3);
    }

    public void OnBattleButton()
    {
        buttonCommonEvent();
        _taps[1].transform.SetSiblingIndex(3);
    }

    public void OnWorkButton()
    {
        buttonCommonEvent();
        _taps[2].transform.SetSiblingIndex(3);
    }

    void buttonCommonEvent()
    {
        if (!isOpen)
        {
            iTween.MoveTo(_Store, iTween.Hash("y", 960f, "time", 1.0f));
            ObjectPack.instance.Get(Managers.LobbyManager).GetComponent<LobbyManager>().moveMoneyWin(isOpen);
            GetComponentInChildren<WorkinMenu>().WeekendPanelHide(_BSM.TodayIsWeekend());
            isOpen = true;
            _escBackBoard.SetActive(true);
        }        
    }

    public void OnCloseStore()
    {
        if (isOpen)
        {
            iTween.MoveTo(_Store, iTween.Hash("y", -570f, "time", 1.0f));
            ObjectPack.instance.Get(Managers.LobbyManager).GetComponent<LobbyManager>().moveMoneyWin(isOpen);
            isOpen = false;
            _escBackBoard.SetActive(false);
        }        
    }
}
