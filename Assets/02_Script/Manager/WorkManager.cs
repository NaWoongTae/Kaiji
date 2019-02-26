using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkManager : MonoBehaviour
{

    [SerializeField] Text daytxt;
    [SerializeField] Text txt;
    [SerializeField] Text workName;
    BaseManager _BSM;

	// Use this for initialization
	void Start ()
    {        
        StartCoroutine(work());
	}
	
	// Update is called once per frame
	IEnumerator work()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        workName.text = _BSM.INWORK.WORK.ToString() + " 작업";

        for (int k = 0; k < _BSM.INWORK.DAY; k++)
        {
            daytxt.text = dayToString(k + 1);
            for (int i = 0; i < 5; i++)
            {        
                for (int j = 0; j < 7; j++)
                {
                    txt.rectTransform.anchoredPosition -= new Vector2(0, 30f);
                    txt.text += "."; 
                    yield return new WaitForSeconds(0.1f);
                }
                txt.text = "작업중";
                txt.rectTransform.anchoredPosition += new Vector2(0, 210f);
            }
        }
        _BSM.NOWUSER.DATE += _BSM.INWORK.DAY;
        yield return new WaitForSeconds(0.5f);
        _BSM.NOWUSER.COIN[(int)_BSM.INWORK.WORK] += 2 * _BSM.INWORK.DAY;

        _BSM._loadLobbyScene("WorkScene");
	}

    string dayToString(int i)
    {
        switch (i)
        {
            case 1:
                return "첫째날";
            case 2:
                return "둘째날";
            case 3:
                return "셋째날";
            case 4:
                return "넷째날";
            case 5:
                return "다섯째날";
            case 6:
                return "여섯째날";
        }
        return null;
    }
}
