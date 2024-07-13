using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public AudioClip inGameClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
 

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx 
    {
        Btn =0,
        CountDown = 1,
        Lose = 2,
        Slot = 3,
        JustScore =4,
        BestScore =5,
        GetBox = 6,
        GiveBox = 7,
        Bomb = 8,
        Car = 9
     
    };

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
        channelIndex = sfxClips.Length;
    }

    private void Start()
    {
        bgmPlayer.Play();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxObject");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            // 현재 채널이 재생 중이 아니면 새로운 사운드 재생
            if (!sfxPlayers[loopIndex].isPlaying)
            {
                channelIndex = loopIndex; // 채널 인덱스 업데이트
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx]; // 새 클립 할당
                sfxPlayers[loopIndex].Play(); // 재생
                break; // 반복 중단
            }
        }
    }
    public void StopSfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {

            if (sfxPlayers[i].clip == sfxClips[(int)sfx] && sfxPlayers[i].isPlaying)
            {
                sfxPlayers[i].Stop();
                break;
            }
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            bgmPlayer.Stop();
            GameObject bgmObject = new GameObject("BgmPlayer");
            bgmObject.transform.parent = transform;
            bgmPlayer = bgmObject.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = inGameClip;

            bgmPlayer.Play();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            bgmPlayer.Stop();
            GameObject bgmObject = new GameObject("BgmPlayer");
            bgmObject.transform.parent = transform;
            bgmPlayer = bgmObject.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmClip;

            bgmPlayer.Play();
        }
    }

    private void OnEnable()
    {
        // SceneManager.sceneLoaded 이벤트에 OnSceneLoaded 메서드를 구독.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // SceneManager.sceneLoaded 이벤트에서 OnSceneLoaded 메서드를 구독 해제.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}