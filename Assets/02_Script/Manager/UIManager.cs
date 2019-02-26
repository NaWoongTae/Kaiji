using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Windows
{
    LoadingWin,    
    joinloginWin,
    MenuWin,
    OptionWin,
    CoinWin,
    BattleRequestWin,
    WorkRequestWin,
    stampWin,
    MessageWin,
    resultWin,
    item3_RequestWin,
    NewsWin,

    Max
}

public class UIManager : TSingleton<UIManager>
{
    Dictionary<Windows, GameObject> WindowDic = new Dictionary<Windows, GameObject>();    

    public GameObject MakeWindow(Windows _win)
    {
        GameObject go;
        if (WindowDic.ContainsKey(_win))
        {
            go = WindowDic[_win];
        }
        else
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("BaseScene"));

            GameObject prefab;
            prefab = Resources.Load("Prefabs\\Windows\\" + _win.ToString()) as GameObject;
            go = Instantiate(prefab);
            imageSort(go, false);
            WindowDic.Add(_win, go);
        }

        return go;
    }

    protected override void Init()
    {
        base.Init();
    }

    void imageSort(GameObject go,bool whatopen, Windows whatWin = Windows.Max)
    {
        if (!whatopen)
            go.GetComponent<Canvas>().sortingOrder = 3;
        else
        {
            switch (whatWin)
            {
                case Windows.BattleRequestWin:
                case Windows.WorkRequestWin:
                case Windows.NewsWin:
                    go.GetComponent<Canvas>().sortingOrder = 6;
                    break;
                case Windows.stampWin:
                case Windows.item3_RequestWin:
                    go.GetComponent<Canvas>().sortingOrder = 7;
                    break;
                case Windows.MessageWin:
                    go.GetComponent<Canvas>().sortingOrder = 8;
                    break;
                default:
                    go.GetComponent<Canvas>().sortingOrder = 5;
                    break;
            }
        }
    }

    public GameObject OpenWIn(Windows _win)
    {
        if (!WindowDic.ContainsKey(_win))
            MakeWindow(_win);
        
        imageSort(WindowDic[_win], true, _win);

        if (!WindowDic[_win].activeSelf)
            WindowDic[_win].SetActive(true);

        return WindowDic[_win];
    }

    public bool isOpen(Windows _win)
    {
        if (WindowDic.ContainsKey(_win))
            if (WindowDic[_win].activeSelf)
                return true;

        return false;
    }

    public void removeWin(Windows _win)
    {
        if (WindowDic.ContainsKey(_win))
            WindowDic.Remove(_win);
    }

    public void closeWin(Windows _win)
    {
        if (WindowDic.ContainsKey(_win))
        {
            imageSort(WindowDic[_win], false);
            WindowDic[_win].SetActive(false);
        }
    }

    public void AllClose()
    {
        foreach (KeyValuePair<Windows,GameObject> go in WindowDic)
        {
            if (go.Key == Windows.LoadingWin) continue;

            if (go.Value.activeSelf)
                go.Value.SetActive(false);
        }
    }
}
