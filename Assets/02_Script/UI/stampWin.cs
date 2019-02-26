using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stampWin : MonoBehaviour {

    [SerializeField] Image stamp;

    RectTransform _recT;
    Animator _animator;

    List<Sprite> windrow;

    void Awake()
    {
        _recT = stamp.GetComponent<RectTransform>();
        _animator = stamp.GetComponent<Animator>();
        windrow = new List<Sprite>();
        windrow.Add(ObjectPack.instance.SPRITEPACK["winStamp"]);
        windrow.Add(ObjectPack.instance.SPRITEPACK["drowStamp"]);
    }    

    public void playAni(string str)
    {
        _animator.SetTrigger(str);
    }

    void transPosition(float x, float y)
    {
        _recT.anchoredPosition = new Vector2(x, y);
    }

    public void battleResult(result i)
    {
        switch (i)
        {
            case result.win:
                transPosition(-175f, 40f);
                playAni("OnStamp");
                stamp.sprite = windrow[0];
                break;
            case result.lose:
                transPosition(-175f, 360f);
                playAni("OnStamp");
                stamp.sprite = windrow[0];
                break;
            case result.drow:
                transPosition(-175f, 200f);
                playAni("OnStamp");
                stamp.sprite = windrow[1];
                break;
        }
    }
}
