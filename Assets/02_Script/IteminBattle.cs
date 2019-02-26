using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IteminBattle : MonoBehaviour
{
    [SerializeField] RectTransform buttonParent;
    [SerializeField] GameObject stageButtonPrefab;
    [Space]
    [SerializeField] float button_h;
    [SerializeField] float blink;
    [SerializeField] float start;
    [SerializeField] float full_box;

    BattleManager _BTM;

    // Use this for initialization
    void Start()
    {
        _BTM = GetComponentInParent<BattleManager>();
        Create(button_h, blink, start, stageButtonPrefab, buttonParent, full_box);
    }    

    List<GameObject> Create(float Box_h, float b, float startPos, GameObject prefab, RectTransform recT, float Full_h = 1260f)
    {
        List<GameObject> btns = new List<GameObject>();
        float HeightandBlick = Box_h + b;
        int whenFull_h_noExtend = (int)((Full_h + b) / HeightandBlick);

        List<int> data = new List<int>();
        
        for (int i = 0; i < DataMng.instance.Get(LowDataType.ITEM).NODE.Count; i++)
        {
            if (DataMng.instance.Get(LowDataType.ITEM).NODE[(i+1).ToString()]["Type"].Equals("ingame"))
            {
                data.Add(i+1);
            }
        }
        int indexCount = data.Count;

        float h = (HeightandBlick * indexCount - b > Full_h) ? HeightandBlick * indexCount - b : Full_h;
        float y = (h - Full_h > 0) ? -1 * (h - Full_h) / 2 : 0;

        recT.anchoredPosition = new Vector2(0, y);
        recT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);

        float origin_pos = startPos + (HeightandBlick / 2) * ((indexCount > whenFull_h_noExtend) ? indexCount - whenFull_h_noExtend : 0);

        for (int i = 0; i < indexCount; i++)
        {
            GameObject go = Instantiate(prefab, recT);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, origin_pos - HeightandBlick * i);
            btns.Add(go);
        }

        for (int i = 0; i < btns.Count; i++)
        {
            btns[i].GetComponent<ItemButtoninBattle>().init(data[i]);
            _BTM.IBB.Add(btns[i].GetComponent<ItemButtoninBattle>());
        }

        return btns;
    }
}
