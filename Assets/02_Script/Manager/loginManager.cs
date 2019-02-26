using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class loginManager : MonoBehaviour
{        
    [SerializeField] Text _logined_id;
    
    BaseManager _BSM;
    loginWin _LGW;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(FacebookInit, WaitUnity);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    void Start()
    {
        _BSM = ObjectPack.instance.Get(Managers.BaseManager).GetComponent<BaseManager>();
        _LGW = UIManager.instance.MakeWindow(Windows.joinloginWin).GetComponent<loginWin>();
    }

    public void startButton()
    {
        if (!_logined_id.text.Equals(string.Empty))
            _BSM._loadLobbyScene("LoginScene");
    }

    public void joinbutton()
    {
        _LGW.openJoinButton();
    }

    public void loginbutton()
    {
        _LGW.openLoginButton();
    }

    public void logined_id(string str)
    {
        _logined_id.text = str;
    }

    void FacebookInit()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("facebook sdk의 초기화에 실패.");
        }
    }

    void WaitUnity(bool isPlay)
    {
        if (!isPlay)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void AuthCallback(ILoginResult result)
    {
        if (result.Error == null)
        {
            AccessToken AT = AccessToken.CurrentAccessToken;
            FB.API("/me?field=first_name", HttpMethod.GET);
            FB.API("/me/picture?type=squar&height=128&weidth=128", HttpMethod.GET);
        }
    }

    public void ClickFacebookLogIn()
    {
        List<string> parms = new List<string>() { "public_profile", "email"};
        FB.LogInWithReadPermissions(parms, AuthCallback);
    }

    public void ClickFacebookLogout()
    {
        FB.LogOut();
    }
}
