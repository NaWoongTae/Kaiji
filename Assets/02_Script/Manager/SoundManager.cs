using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : TSingleton<SoundManager> {

    public enum TYPEBGM
    {
        LOGIN,
        LOBBY,
        BATTLE,
        WORK,

        MAX
    }

    public enum TYPEEFFECT
    {
        WINDOW,

        MAX
    }

    // [SerializeField] AudioClip[] _BGMArry;
    List<AudioClip> _ltBGM;
    List<AudioClip> _ltEFFECT;

    List<AudioSource> _used;

    AudioSource _asBGM;

    void Awake()
    {
        gameObject.AddComponent<AudioListener>();
        _asBGM = gameObject.AddComponent<AudioSource>();
        _ltBGM = new List<AudioClip>();
        _ltEFFECT = new List<AudioClip>();
        _used = new List<AudioSource>();
        LoadSound();
        LoadEffSound();
    }

    // Update is called once per frame
    void Update()
    {
        EraseUsed();
    }

    public void EraseUsed()
    {
        // list같은 애들은 remove를 해줄때 내부적으로 변화?가 있기때문에
        // 한루프에 1번씩) remove를 했다면 break;를 걸고 다음 remove는 다음 루틴에서 하도록 하는것이 좋다.  
        if (_used.Count > 0)
        {
            foreach (AudioSource item in _used)
            {
                if (!item.isPlaying)
                {
                    Destroy(item.gameObject);
                    _used.Remove(item);
                    break;
                }
            }
        }
    }

    public void LoadSound()
    {
        AudioClip ac;
        for (int i = 0; i < (int)TYPEBGM.MAX; i++)
        {
            string path = "Sound\\" + ((TYPEBGM)i).ToString();
            ac = Resources.Load(path) as AudioClip;
            _ltBGM.Add(ac);
        }
    }

    public void LoadEffSound()
    {
        AudioClip ac;
        for (int i = 0; i < (int)TYPEEFFECT.MAX; i++)
        {
            string path = "Sound\\" + ((TYPEEFFECT)i).ToString();
            ac = Resources.Load(path) as AudioClip;
            _ltEFFECT.Add(ac);
        }
    }

    public void playBGMsound(TYPEBGM type, float volume = 1.0f, bool isLoop = true)
    {
        _asBGM.clip = _ltBGM[(int)type];
        _asBGM.volume = volume;
        _asBGM.loop = isLoop;

        _asBGM.Play();
    }

    public void playEFFECTsound(TYPEEFFECT type, float volume = 0.2f, bool isLoop = false)
    {
        GameObject eff = new GameObject();
        eff.transform.parent = transform;
        AudioSource _eff = eff.AddComponent<AudioSource>();

        _eff.clip = _ltEFFECT[(int)type];
        _eff.volume = volume;
        _eff.loop = isLoop;

        _eff.Play();
        _used.Add(_eff);
    }

    public void stopMusic()
    {
        _asBGM.Stop();
    }

    public void ChangeVol(float volume = 0.5f)
    {
        _asBGM.volume = volume;
    }
}
