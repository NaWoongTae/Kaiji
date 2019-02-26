using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loginWin : MonoBehaviour {

    [SerializeField] GameObject join;
    [SerializeField] GameObject login;

    [SerializeField] InputField IF_join_ID;
    [SerializeField] InputField IF_join_PW;
    [SerializeField] InputField IF_login_ID;
    [SerializeField] InputField IF_login_PW;
    
    BaseManager _BSM;
    loginManager _LGM;

    private void Awake()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _LGM = ObjectPack.instance.Get(Managers.LoginManager).GetComponent<loginManager>();

        join.SetActive(false);
        login.SetActive(false);

        IF_join_ID.text = string.Empty;
        IF_join_PW.text = string.Empty;
        IF_login_ID.text = string.Empty;
        IF_login_PW.text = string.Empty;
    }

    public void joinID()
    {
        // SoundManager.instance.playEffectsound(SoundManager.TYPEEFFECT.button);
        if (!IF_join_ID.text.Equals(string.Empty))
        {
            if (!IF_join_PW.text.Equals(string.Empty))
            {
                bool noWhere = true;
                if (_BSM.USERINFO.ContainsKey(IF_join_ID.text))
                {
                    _BSM.ShowMessage("이미 있어");
                    noWhere = false;
                }

                if (noWhere)
                {
                    if (_BSM.USERINFO.Count == 0)
                        _BSM.USERINFO.Add(IF_join_ID.text, new UserInfo(IF_join_ID.text, IF_join_PW.text, 1));
                    else
                        _BSM.USERINFO.Add(IF_join_ID.text, new UserInfo(IF_join_ID.text, IF_join_PW.text));

                    _BSM.SaveUserData();

                    cancelButton();
                }
            }
            else
                _BSM.ShowMessage("비밀번호를 써줘");
        }
        else
            _BSM.ShowMessage("가입할 아이디를 써줘");
    }

    public void loginID()
    {
        if (!IF_login_ID.text.Equals(string.Empty))
        {
            if (!IF_login_PW.text.Equals(string.Empty))
            {                
                if (_BSM.USERINFO.ContainsKey(IF_login_ID.text))
                {
                    if (_BSM.USERINFO[IF_login_ID.text].PW.Equals(IF_login_PW.text))
                    {
                        // _BSM.NOWUSER = new UserInfo();
                        _BSM.NOWUSER = _BSM.USERINFO[IF_login_ID.text];

                        for (int i = 0; i < 3; i++)
                        {
                            _BSM.PRICE_RECORD[i] = _BSM.NOWUSER.COINPRICE[i];
                            _BSM.PRICE[i] = (int)(_BSM.PRICE_RECORD[i][4]); ;
                        }
                        _BSM.SaveUserData();

                        _LGM.logined_id(IF_login_ID.text);
                        cancelButton();
                    }
                    else
                        _BSM.ShowMessage("비밀번호가 틀린듯");
                }
                else
                    _BSM.ShowMessage("그런 아이디가 없어");                
            }
            else
                _BSM.ShowMessage("가입한 비밀번호를 써줘");
        }
        else
            _BSM.ShowMessage("가입한 아이디를 써줘");
    }

    public void cancelButton()
    {
        // SoundManager.instance.playEffectsound(SoundManager.TYPEEFFECT.button);
        join.SetActive(false);
        login.SetActive(false);
        IF_join_ID.text = string.Empty;
        IF_join_PW.text = string.Empty;
        IF_login_ID.text = string.Empty;
        IF_login_PW.text = string.Empty;
    }

    public void openJoinButton()
    {
        // SoundManager.instance.playEffectsound(SoundManager.TYPEEFFECT.button
        join.SetActive(true);
    }

    public void openLoginButton()
    {
        // SoundManager.instance.playEffectsound(SoundManager.TYPEEFFECT.button);
        login.SetActive(true);
    }    
}
