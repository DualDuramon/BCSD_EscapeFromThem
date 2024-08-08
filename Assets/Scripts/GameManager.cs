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
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private StatusSaveData StatusSaveData = new StatusSaveData();

    //보너스 지급 관련
    [SerializeField] private GameObject myUI;
    public bool didPlayerGetBonus = true;

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
        Debug.Log("플레이어 게임매니저 어웨잌");
    }

    private void Start()
    {
        FindObjectsOfThisMap();
        RespawnPlayer();
    }

    public void LoadNextScene()        //다음 씬 호출 함수
    {
        SaveNowPlayerStatus();
        StartCoroutine(LoadNextStageCoroutine());
    }

    private IEnumerator LoadNextStageCoroutine()        //씬 로딩 코루틴
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(++nowStageIndex);
        asyncLoad.allowSceneActivation = false;
        ActiveBonusPanel();

        while (!asyncLoad.isDone && !didPlayerGetBonus)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        FindObjectsOfThisMap();
        RespawnPlayer();
    }

    private void SaveNowPlayerStatus()  //플레이어 스테이터스 임시 저장 함수
    {
        player.GetComponent<PlayerStatus>().SaveMyStatus(ref StatusSaveData);
    }

    public void LoadPlayerStatus()      //임시 플레이어 스테이터스 데이터 로드 함수
    {
        player.GetComponent<PlayerController>().ResetStatus(ref StatusSaveData);
    }

    private void FindObjectsOfThisMap() //맵 상에 존재하는 필요한 오브젝트들 찾기 함수
    {
        player = GameObject.FindGameObjectWithTag("Player");                        //플레이어 찾기
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;   //플레이어 리스폰지점 찾기
        myUI = GameObject.FindWithTag("Canvas");                //보너스 패널 찾기
    }

    private void RespawnPlayer()    //플레이어 리스폰 함수
    {
        LoadPlayerStatus();
        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = playerSpawnPoint.rotation;
    }

    private void ActiveBonusPanel()   //보너스 패널 활성화 함수
    {
        didPlayerGetBonus = false;
        myUI.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void StageBonus(string type)    //보너스 지급 함수
    {
        switch (type)
        {
            case "GRENADE": //수류탄 보너스
                player.GetComponent<PlayerStatus>().IncreaseGrenade(1);
                break;
            case "AMMO": //총알 보너스
                player.GetComponent<PlayerStatus>().Increase_bullet(90);
                break;
            case "HEAL": //체력 보너스
                player.GetComponent<PlayerStatus>().MakeNowHpMax();
                break;
            
            default:
                player.GetComponent<PlayerStatus>().IncreaseGrenade(1);
                Debug.Log("보너스 타입이 잘못되었습니다. -> 수류탄 보너스 지급");
                break;
        }

        myUI.transform.GetChild(0).gameObject.SetActive(false);
        didPlayerGetBonus = true;
    }
}