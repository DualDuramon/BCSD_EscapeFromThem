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
    public int nowStageIndex = 0;
    
    //플레이어 관련
    [SerializeField] private GameObject player;
    [SerializeField] private StatusSaveData currentSaveData = new StatusSaveData();
    public StatusSaveData CurrentSaveData { get => currentSaveData; set { currentSaveData = value; } }
    public int zombieKills = 0;
    
    //보너스 지급 관련
    [SerializeField] private int bonusGrenade = 1;
    [SerializeField] private int bonusAmmo = 90;

    //캔버스 관련
    [SerializeField] private UIManager myUI;                       //캔버스 오브젝트
    
    //그외
    public bool isPause = false;
    public SaveLoad saveLoadManager;
    public int minScore = 999999;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !myUI.BonusPanel.activeSelf)
        {
            isPause = !isPause;
            myUI.ShowPauseMenuPanel(isPause);
        }
    }

    public void LoadNextScene()  //다음 씬 로딩 함수
    {
        if(nowStageIndex == 0)  //타이틀 신에서 호출할 경우
        {
            ClearCurrentSaveData();
            SoundManager.Instance.PlayBGM(++nowStageIndex);
            SceneManager.LoadScene(nowStageIndex);
        }
        else if(nowStageIndex < SceneManager.sceneCountInBuildSettings - 2) //게임 스테이지에서 호출할 경우
        {
            isPause = true;
            Time.timeScale = 0.0f;
            StartCoroutine(LoadNextStageCoroutine());
        }
        else
        {
            int index = (++nowStageIndex) % SceneManager.sceneCountInBuildSettings;
            SaveNowPlayerStatus();
            SceneManager.LoadSceneAsync(index);
            SoundManager.Instance.PlayBGM(index);
        }
    }

    public void LoadCurrentScene()    //세이브된 씬 로딩 함수
    {
        SoundManager.Instance.PlayBGM(nowStageIndex);
        SceneManager.LoadSceneAsync(nowStageIndex);
    }

    public void GoToTitleScene()
    {
        nowStageIndex = 0;
        SoundManager.Instance.PlayBGM(0);
        SceneManager.LoadSceneAsync(0);
    }

    private IEnumerator LoadNextStageCoroutine()        //씬 로딩 코루틴
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(++nowStageIndex);

        asyncLoad.allowSceneActivation = false;
        myUI.ShowBonusPanel(true);
        CurrentSaveData.totalZombieKills += zombieKills;
        zombieKills = 0;
        
        while (isPause)
        {
            yield return null;
        }

        SaveNowPlayerStatus();
        saveLoadManager.SaveData();
        SoundManager.Instance.PlayBGM(nowStageIndex);
        asyncLoad.allowSceneActivation = true;

    }

    private void SaveNowPlayerStatus()  //플레이어 스테이터스 임시 저장 함수
    {
        player.GetComponent<PlayerStatus>().SaveMyStatus(ref currentSaveData);
        currentSaveData.nowStageIndex = nowStageIndex;
    }

    private void FindObjectsOfThisMap()     //맵 상에 존재하는 필요한 오브젝트들 찾기 함수
    {
        player = GameObject.FindGameObjectWithTag("Player");                        //플레이어 찾기
        myUI = FindAnyObjectByType<UIManager>();                                    //보너스 패널 찾기
    }

    public void StageBonus(string type)     //보너스 지급 함수  -> 보너스 패널 버튼에서 호출
    {
        switch (type)
        {
            case "GRENADE": //수류탄 보너스
                player.GetComponent<PlayerStatus>().Calculate_Grenade(bonusGrenade);
                break;
            case "AMMO": //총알 보너스
                player.GetComponent<PlayerStatus>().Increase_ServedBullet(bonusAmmo);
                break;
            case "HEAL": //체력 보너스
                player.GetComponent<PlayerStatus>().MakeNowHpMax();
                break;
            
            default:
                player.GetComponent<PlayerStatus>().Increase_ServedBullet(bonusAmmo);
                Debug.Log("보너스 타입이 잘못되었습니다. -> 총알 추가 지급");
                break;
        }
        player.GetComponent<PlayerStatus>().Increase_WalkSpeed(1.0f);
        isPause = false;
        Time.timeScale = 1.0f;
    }

    public void ActiveRetryButton()         //재시작 버튼 활성화
    {
        myUI.ShowGameOverRetryButton(true);
    }

    public void ResetStageUIs()             //UI매니저의 텍스트 초기화 함수 호출
    {
        myUI.ResetAllTexts(ref currentSaveData);
    }

    private void ClearCurrentSaveData()     //임시 세이브 데이터 삭제
    {
        nowStageIndex = 0;
        currentSaveData.ResetData();
    }
}