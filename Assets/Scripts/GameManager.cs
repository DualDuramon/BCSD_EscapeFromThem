using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            return instance;
        }
    }
    #endregion
    
    //씬 관련
    private int nowStageIndex = 0;
    
    //플레이어 관련
    [SerializeField] private GameObject player;
    [SerializeField] private StatusSaveData currentSaveData = new StatusSaveData();
    public StatusSaveData CurrentSaveData 
    {
        get => currentSaveData;
    }

    //보너스 지급 관련
    [SerializeField] private int bonusGrenade = 1;
    [SerializeField] private int bonusAmmo = 90;
    public bool isPause = false;

    //캔버스 관련
    [SerializeField] private GameObject myUI;                       //캔버스 오브젝트
    private enum CanvasChild { BONUS_PANEL = 0, RETRY_BUTTON = 1 }  //캔버스 자식 오브젝트 인덱스

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;  //씬 로드시 필요한 함수들 실행
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        FindObjectsOfThisMap(); //맵에 필요한 오브젝트 검색
        //RespawnPlayer();        //플레이어 리스폰
    }

    public void LoadNextScene()        //다음 씬 호출 함수
    {
        isPause = true;
        Time.timeScale = 0.0f;
        StartCoroutine(LoadNextStageCoroutine());
    }

    private IEnumerator LoadNextStageCoroutine()        //씬 로딩 코루틴
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(++nowStageIndex);
        asyncLoad.allowSceneActivation = false;
        ActiveBonusPanel();

        while (isPause)
        {
            yield return null;
        }

        SaveNowPlayerStatus();
        asyncLoad.allowSceneActivation = true;
    }

    private void SaveNowPlayerStatus()  //플레이어 스테이터스 임시 저장 함수
    {
        player.GetComponent<PlayerStatus>().SaveMyStatus(ref currentSaveData);
    }

    public void LoadPlayerStatus()      //임시 플레이어 스테이터스 데이터 로드 함수
    {
        player.GetComponent<PlayerController>().ResetProperties();
    }

    private void FindObjectsOfThisMap() //맵 상에 존재하는 필요한 오브젝트들 찾기 함수
    {
        player = GameObject.FindGameObjectWithTag("Player");                        //플레이어 찾기
        myUI = GameObject.FindWithTag("Canvas");                                    //보너스 패널 찾기
    }

    private void ActiveBonusPanel()   //보너스 패널 활성화 함수
    {
        myUI.transform.GetChild((int)CanvasChild.BONUS_PANEL).gameObject.SetActive(true);  //보너스 패널 가져오기
    }

    public void StageBonus(string type)    //보너스 지급 함수  -> 보너스 패널 버튼에서 호출
    {
        switch (type)
        {
            case "GRENADE": //수류탄 보너스
                player.GetComponent<PlayerStatus>().IncreaseGrenade(bonusGrenade);
                break;
            case "AMMO": //총알 보너스
                player.GetComponent<PlayerStatus>().Increase_bullet(bonusAmmo);
                break;
            case "HEAL": //체력 보너스
                player.GetComponent<PlayerStatus>().MakeNowHpMax();
                break;
            
            default:
                player.GetComponent<PlayerStatus>().IncreaseGrenade(bonusAmmo);
                Debug.Log("보너스 타입이 잘못되었습니다. -> 총알 추가 지급");
                break;
        }

        isPause = false;
        Time.timeScale = 1.0f;
    }

    public void ActiveRetryButton()     //재시작 버튼 활성화
    {
        myUI.transform.GetChild((int)CanvasChild.RETRY_BUTTON).gameObject.SetActive(true);
    }
}