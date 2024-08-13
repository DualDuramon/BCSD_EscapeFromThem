using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //프리펩 및 플레이어 
    [SerializeField] private GameObject prefab_Zombie;              //좀비 프리펩
    [SerializeField] private GameObject prefab_TrueEscapeZone;      //탈출 지점 프리펩
    [SerializeField] private GameObject[] prefab_FalseEscapeZone;   //가짜 탈출지점 프리펩
    [SerializeField] private GameObject player;                     //맵 상에 있는 플레이어 -> 연결해줄 것.

    //스폰 포인트
    [SerializeField] private Transform[] zombieSpawnPoints;         //좀비 스폰 지점s
    [SerializeField] private Transform[] escapePoints;              //탈출 지점s
    [SerializeField] private Transform playerSpawnPoint;            //플레이어 스폰 지점

    //좀비 스폰 제한 변수들
    [SerializeField] private float spawnCoolTime = 0.0f;            //최대 스폰 쿨타임
    private float nowSpawnCoolTime = 0.0f;                          //현재 스폰 쿨타임
    [SerializeField] private int maxZombieCount = 50;               //최대 좀비 수
    private int nowZombieCount = 0;                                 //현재 좀비 수
    [SerializeField] private int firstSpawnZombieCount = 0;         //첫 좀비 스폰 수

    private void Awake()
    {
        SpawnEscapeZone();
        InstantiateZombies();
    }

    private void SpawnEscapeZone()  //탈출지점 생성 함수
    {
        int selectIndex = Random.Range(0, escapePoints.Length);
        for (int i = 0; i < escapePoints.Length; i++)
        {
            if(i != selectIndex)
            {
                Instantiate(
                    prefab_FalseEscapeZone[Random.Range(0,prefab_FalseEscapeZone.Length)], 
                    escapePoints[i]
                    );
            }
            else
            {
                Instantiate(prefab_TrueEscapeZone, escapePoints[i]);
            }
        }
    }

    private void Start()
    {
        RespawnPlayer();
        SpawnZombie_Immediately(firstSpawnZombieCount);
    }

    private void Update()
    {
        TrySpawnZombie();
    }

    public void RespawnPlayer()    //플레이어 리스폰 함수
    {
        player.GetComponent<PlayerController>().ResetProperties();
        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = playerSpawnPoint.rotation;
        GameManager.Instance.ResetStageUIs();
    }

    private void InstantiateZombies()   //좀비 오브젝트 풀링 함수
    {
        for (int i = 0; i < maxZombieCount; i++)
        {
            Instantiate(prefab_Zombie, gameObject.transform).SetActive(false);
        }
    }

    private void TrySpawnZombie()   //좀비 스폰 시도 함수
    {
        if (nowSpawnCoolTime > spawnCoolTime && nowZombieCount < maxZombieCount)
        {
            SpawnZombie();
        }
        else
        {
            nowSpawnCoolTime += Time.deltaTime;
            return;
        }
    }

    private void SpawnZombie()  //좀비 스폰 함수
    {
        GameObject zombie = FindDisabledZombie();
        if(zombie == null)
        {
            Debug.Log("Faild To Spawn Zombie : null");
            return;
        }
        int select = Random.Range(0, zombieSpawnPoints.Length);
        zombie.SetActive(true);
        zombie.transform.position 
            = zombieSpawnPoints[select].position + new Vector3(Random.Range(0, 0.5f), 0, Random.Range(0, 0.5f));
        nowZombieCount++;
        nowSpawnCoolTime = 0.0f;
    }

    private void SpawnZombie_Immediately(int count) //좀비 즉시 스폰 함수
    {
        for (int i = 0; i < count; i++)
            SpawnZombie();
    }

    private GameObject FindDisabledZombie()     //스폰 안된 좀비 반환 함수
    {
        for(int i = 0; i< maxZombieCount; i++)
        {
            if(!transform.GetChild(i).gameObject.activeSelf)
                return transform.GetChild(i).gameObject;
        }
        return null;
    }
    
    public void ZombieDead(GameObject DiedZombie)  //좀비 수 감소 함수
    {
        DiedZombie.SetActive(false);
        nowZombieCount--;
        GameManager.Instance.zombieKills++;
    }

    public void ClearAllZombies()       //모든 좀비 제거 함수
    {
        for(int i = 0; i < maxZombieCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        nowZombieCount = 0;
    }
}