using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 나의 시트명 Enum
public enum LowDataType
{
	ITEM,
	COININFO,
    STAGE,
    BOSSINFO,
    PROBABILLITYITEM,

    MAX
}

public class DataMng : TSingleton< DataMng > 
{
    // LowDataType과 그 시트의 LowBase(내용?)
	Dictionary< LowDataType, LowBase > dataList = new Dictionary<LowDataType, LowBase>();
        
    // TSingleton Init
    protected override void Init()
	{
		base.Init ();
    }
	
    // 
	public LowBase Load<T>( LowDataType type ) where T: LowBase, new()
	{
        // 이미 데이터가 존재하고 있다면, 함수를 종료한다.
        if (dataList.ContainsKey (type))
		{
			LowBase lowBase = dataList[ type ]; // 있는내용 넣어준다
			return dataList[ type ]; // 있는거 반환
		}
		
		TextAsset textAsset = Resources.Load ("Bin/" + type.ToString ()) as TextAsset; // textAsset
		
		if( textAsset != null )
		{
			T t = new T ();
			t.Load (textAsset.text); // textAsset.text은 이미 한번 변환된 데이터
            dataList.Add( type, t ); // 처리후 추가
		}
		
		return dataList [type];
	}

    // 로드
	public void LoadData()
	{
        Load<Item>(LowDataType.ITEM);
        Load<Coininfo> (LowDataType.COININFO);
        Load<Stage>(LowDataType.STAGE);
        Load<Bossinfo>(LowDataType.BOSSINFO);
        Load<ProbabillityItem>(LowDataType.PROBABILLITYITEM);
    }
	
    // 있으면 가져오기
	public  LowBase Get( LowDataType dataType ) 
	{
		if( dataList.ContainsKey ( dataType ) )
			return dataList[ dataType ];
		
		return null;
	}

    // clear
	public void Clear ()
	{
		dataList.Clear ();
	}

    //public LowBase _readData(LowDataType dataType)
    //{
    //    return dataList[dataType];
    //}
}



