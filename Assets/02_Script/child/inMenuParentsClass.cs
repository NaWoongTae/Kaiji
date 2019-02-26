using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inMenuParentsClass : MonoBehaviour {

    // A+b > (h + b)x
    // int x = (int)((A+b)/(h+b)) 전체 크기가 확장되지않는 최대버튼개수

    /// <summary>
    /// 데이터 개수에 따른 버튼 동적생성
    /// width는 미리 정해두고 Height만 가지고 작업
    /// </summary>
    /// <param name="Box_h">생성할 버튼 높이</param>
    /// <param name="b">버튼 사이의 간격</param>
    /// <param name="startPos">시작위치</param>
    /// <param name="data">데이터 길이(버튼개수)</param>
    /// <param name="prefab">생성할 버튼 Prefabs</param>
    /// <param name="recT">버튼의 부모transform</param>
    /// <param name="Full_h">버튼이 들어갈 스크롤의 높이</param>
    protected virtual List<GameObject> Create(float Box_h, float b, float startPos, LowDataType data, GameObject prefab, RectTransform recT, float Full_h = 1260f)
    {
        List<GameObject> btns = new List<GameObject>();
        float HeightandBlick = Box_h + b;
        int whenFull_h_noExtend = (int)((Full_h + b) / HeightandBlick);

        int indexCount = DataMng.instance.Get(data).NODE.Count;
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

        return btns;
    }
}
