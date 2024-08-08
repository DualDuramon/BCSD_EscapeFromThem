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

    public void LoadNextSceneScene()        //다음 씬 호출 함수
    {
        SaveNowPlayerStatus();
        StartCoroutine(LoadNextStageCoroutine());
    }

    private IEnumerator LoadNextStageCoroutine()        //씬 로딩 코루틴
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(++nowStageIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        FindObjectsOfThisMap();
        RespawnPlayer();
    }

    private void SaveNowPlayerStatus()  //플레이어 스테이터스 임시 저장 함수
    {
        player.GetComponent<PlayerStatus>().SaveMyStatus(ref StatusSaveData);
    }

    public void LoadPlayerStatus()      //임시 플레이어 스테이터스 데이터 로드 함수
    {
        player.GetComponent<PlayerStatus>().ResetStatus(StatusSaveData);
    }

    private void FindObjectsOfThisMap() //맵 상에 존재하는 필요한 오브젝트들 찾기 함수
    {
        player = GameObject.FindGameObjectWithTag("Player");                        //플레이어 찾기
        playerSpawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;   //플레이어 리스폰지점 찾기
    }

    private void RespawnPlayer()    //플레이어 리스폰 함수
    {
        LoadPlayerStatus();
        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = playerSpawnPoint.rotation;
    }

}