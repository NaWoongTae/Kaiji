using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

public class Item : LowBase {

    // 해당 시트의 column명
    public enum Index
    {
        Index,
        Name,
        Explan,
        Price,
        Duration,
        Type,
        Cost,
        max
    }

    public string mainKey = "Index";

    public override void Load(string strJson)
    {
        // 이 node는 이미 한번 변환된 데이터
        JSONNode node = JSONNode.Parse(strJson);

        // column의 개수만큼
        for (int i = 0; i < (int)Index.max; i++)
        {
            Index subKey = EnumHelper.StringToEnum<Index>(i.ToString()); // Enum = Enum <- (int -> string);
            if (string.Compare(mainKey, subKey.ToString()) != 0) // subkey가 index가 아닐때
            {
                // 시트별로 분할하기 때문에 node[x]는 항상 x == 0
                for (int j = 0; j < node[0].AsArray.Count; j++) // node[0].AsArray.Count 줄의 개수?
                    Add(node[0][j][mainKey],                    // 키
                        subKey.ToString(),                      // 서브 키
                        node[0][j][subKey.ToString()].Value);   // 값
            }
        }
    }
}
