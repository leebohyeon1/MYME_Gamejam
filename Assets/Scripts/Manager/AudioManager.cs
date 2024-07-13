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
        }

        Init();

    }

    private void Start()
    {
        bgmPlayer.Play();
    }

    void Init()
    {
        //����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            Debug.Log(PlayerPrefs.GetFloat("BGMVolume"));
            bgmPlayer.volume = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            bgmPlayer.volume = bgmVolume;
        }

        //ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxObject");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                sfxPlayers[index].volume = PlayerPrefs.GetFloat("SFXVolume");
            }
            else
            {
                sfxPlayers[index].volume = sfxVolume;
            }
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            // ���� ä���� ��� ���� �ƴϸ� ���ο� ���� ���
            if (!sfxPlayers[loopIndex].isPlaying)
            {
                channelIndex = loopIndex; // ä�� �ε��� ������Ʈ
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx]; // �� Ŭ�� �Ҵ�
                sfxPlayers[loopIndex].Play(); // ���
                break; // �ݺ� �ߴ�
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
            bgmPlayer.clip = inGameClip;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            bgmPlayer.clip = bgmClip;
        }
    }

    public void ChangeBGM(float value)
    {
        bgmPlayer.volume = value;
    }

    public void ChangeSFX(float value)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i].volume = value;
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmPlayer.volume);
        PlayerPrefs.SetFloat("SFXVolume", sfxPlayers[0].volume);
    }
    private void OnEnable()
    {
        // SceneManager.sceneLoaded �̺�Ʈ�� OnSceneLoaded �޼��带 ����.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // SceneManager.sceneLoaded �̺�Ʈ���� OnSceneLoaded �޼��带 ���� ����.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}