using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchinMenu : inMenuParentsClass
{
    [SerializeField] RectTransform buttonParent;
    [SerializeField] GameObject stageButtonPrefab;
    
    // Use this for initialization
    void Start ()
    {
        Create(290f, 30f, 474f, LowDataType.STAGE, stageButtonPrefab, buttonParent);
    }

    protected override List<GameObject> Create(float Box_h, float b, float startPos, LowDataType data, GameObject prefab, RectTransform recT, float Full_h = 1260f)
    {        
        List<GameObject> btns = base.Create(Box_h, b, startPos, data, prefab, recT, Full_h);

        for (int i = 0; i < btns.Count; i++)
        {            
            btns[i].GetComponent<StageButton>().init(i + 1);
        }
        btns[btns.Count - 1].GetComponent<StageButton>().ISBOSS = true;
        return btns;
    }

    /*void Create()
    {
        int indexCount = DataMng.instance.Get(LowDataType.STAGE).NODE.Count;
        float h = (320f * indexCount - 30f > 1260f) ? 320f * indexCount - 30f : 1260f;
        float y = (h - 1260f > 0) ? -1 * (h - 1260f) / 2 : 0;
        buttonParent.anchoredPosition = new Vector2(0, y);
        buttonParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
        float origin_pos = 474f + 160f * ((indexCount > 4) ? indexCount - 4 : 0);
        for (int i = 0; i < indexCount; i++)
        {
            GameObject go = Instantiate(stageButtonPrefab, buttonParent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, origin_pos - 320f * i);
            go.GetComponent<StageButton>().init(i + 1);
        }
    }*/

    
}
