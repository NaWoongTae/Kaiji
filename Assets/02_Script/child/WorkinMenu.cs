using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkinMenu : inMenuParentsClass
{

    WorkRequestWin WRW;
    [SerializeField] Image WeekendPanel;

    public void WeekendPanelHide(bool week)
    {
        if (!week)
            WeekendPanel.gameObject.SetActive(false);
        else
            WeekendPanel.gameObject.SetActive(true);
    }

    public void WorkButtonRock()
    {
        WRW = UIManager.instance.OpenWIn(Windows.WorkRequestWin).GetComponent<WorkRequestWin>();
        WRW.init(inWork.work.채광);
    }

    public void WorkButtonSissiors()
    {
        WRW = UIManager.instance.OpenWIn(Windows.WorkRequestWin).GetComponent<WorkRequestWin>();
        WRW.init(inWork.work.공장);
    }

    public void WorkButtonPaper()
    {
        WRW = UIManager.instance.OpenWIn(Windows.WorkRequestWin).GetComponent<WorkRequestWin>();
        WRW.init(inWork.work.방직);
    }
}
