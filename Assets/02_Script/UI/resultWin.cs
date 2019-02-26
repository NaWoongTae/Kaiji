using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultWin : MonoBehaviour {

    Animator ani;
    result res;
    List<Sprite> result_sprite;

    void Awake()
    {
        ani = GetComponentInChildren<Animator>();
        result_sprite = new List<Sprite>();
        for (int i = 1; i <= 6; i++)
        {
            result_sprite.Add(ObjectPack.instance.SPRITEPACK["result_" + i]);
        }
    }    

	public void init(result i)
    {
        res = i;

        Sprite inSprite = null;
        switch (i)
        {
            case result.win:
                inSprite = result_sprite[Random.Range(0, 3)];
                break;
            case result.drow:
                inSprite = result_sprite[3];
                break;
            case result.lose:
                inSprite = result_sprite[Random.Range(4, 6)];
                break;
        }
        ani.GetComponent<Image>().sprite = inSprite;
	}
	
	// Update is called once per frame
	public void playAni ()
    {
        switch (res)
        {
            case result.win:
                ani.SetTrigger("win");
                break;
            case result.drow:
                ani.SetTrigger("drow");
                break;
            case result.lose:
                ani.SetTrigger("lose");
                break;
        }
	}    
}
