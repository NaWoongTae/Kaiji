using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionWin : MonoBehaviour
{
    [SerializeField] Slider _slider;
    BaseManager _BSM;

	// Use this for initialization
	void Awake ()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_slider.value != _BSM.NOWUSER.SOUNDVOLUM)
            SoundManager.instance.ChangeVol(_slider.value);
    }

    public void optionSave()
    {
        _BSM.NOWUSER.SOUNDVOLUM = _slider.value;
        _BSM.SaveUserData();
        cancel();
    }

    public void cancel()
    {
        gameObject.SetActive(false);
    }

    public void openOption()
    {
        _slider.value = _BSM.NOWUSER.SOUNDVOLUM;
        gameObject.SetActive(true);
    }
}
