using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    //싱글톤
    static private SoundManager instance;
    static public SoundManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SoundManager>();
            }

            return instance;
        }
    }

    [Header("BGM")] //BGM 관련
    [SerializeField] private AudioClip[] bgmClip;         //배경음악 클립
    [SerializeField] private AudioSource bgmPlayer;     //배경음악 소스
    [SerializeField] private float bgmVolume = 1.0f;    //배경음악 볼륨

    [Header("SFX")] //SFX 관련
    [SerializeField] private AudioClip[] sfxClips;      //효과음 클립
    [SerializeField] private int channels = 16;         //채널 수
    [SerializeField] private float sfxVolume = 1.0f;    //효과음 볼륨
    private AudioSource[] sfxPlayers;                   //효과음 재생기 숫자
    private int channelIndex = 0;                       //현재 효과음 재생기 인덱스

    public enum SFXPlayerType {
        PlayerFire,
        PlayerDeath,
        GrenadeExplosion,
        PlayerReload,
        ZombieDeath,
        ZombieHit,
        ZombieAttack
    }
    

    private void Awake()
    {
        //싱글톤 파트
        if(instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }

        //인스턴스 초기화
        InitiateSounds();
    }

    private void Start()
    {
        bgmPlayer.Play();
    }

    private void FixedUpdate()
    {
        bgmPlayer.volume = bgmVolume * 0.5f;
    }

    private void InitiateSounds()
    {
        //BGM 생성 및 초기화
        GameObject Obj = new GameObject("BGM_Player");
        Obj.transform.SetParent(transform);
        bgmPlayer = Obj.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip[0];

        //SFX 생성 및 초기화
        GameObject sfxObj = new GameObject("SFX_Player");
        sfxObj.transform.SetParent(transform);
        sfxPlayers = new AudioSource[channels];

        for (int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            sfxPlayers[idx] = sfxObj.AddComponent<AudioSource>();
            sfxPlayers[idx].loop = false;
            sfxPlayers[idx].playOnAwake = false;
            sfxPlayers[idx].volume = sfxVolume;
        }
    }

    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void PlayBGM(int index)
    {
        bgmPlayer.Stop();
        bgmPlayer.clip = (index >= bgmClip.Length) ? bgmClip[bgmClip.Length - 1] : bgmClip[index];
        bgmPlayer.Play();
    }

    public void PlaySFX(SFXPlayerType type)
    {
        for (int i = 0, loopIndex; i < sfxPlayers.Length; i++) 
        {
            loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) continue;

            channelIndex = loopIndex;
            sfxPlayers[channelIndex].clip = sfxClips[(int)type];
            sfxPlayers[channelIndex].Play();
            break;
        }
    }
}