using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //프리펩 및 플레이어 
    [SerializeField] private GameObject prefab_Zombie;
    [SerializeField] private GameObject prefab_TrueEscapeZone;
    [SerializeField] private GameObject[] prefab_FalseEscapeZone;
    [SerializeField] private GameObject player;

    //스폰 포인트
    [SerializeField] private Transform[] zombieSpawnPoints;
    [SerializeField] private Transform[] escapePoints;
    [SerializeField] private Transform playerSpawnPoint;

    //좀비 스폰 제한 변수들
    [SerializeField] private float spawnCoolTime = 0.0f;
    private float nowSpawnCoolTime = 0.0f;
    [SerializeField] private int maxZombieCount = 50;
    private int nowZombieCount = 0;

    private void Awake()
    {
        SpawnEscapeZone();
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
    }

    private void Update()
    {
        TrySpawnZombie();
    }

    private void RespawnPlayer()    //플레이어 리스폰 함수
    {
        GameManager.Instance.LoadPlayerStatus();
        player.transform.position = playerSpawnPoint.position;
        player.transform.rotation = playerSpawnPoint.rotation;
    }

    private void TrySpawnZombie()
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

    private void SpawnZombie()  //좀비 생성 함수
    {
        //난수생성
        //스폰지점 선택
        //좀비 생성
        //좀비 카운트 업
        //좀비 쿨타임 초기화;
        int select = Random.Range(0, zombieSpawnPoints.Length);
        Instantiate(prefab_Zombie, zombieSpawnPoints[select].position, Quaternion.identity);
        nowZombieCount++;
        nowSpawnCoolTime = 0.0f;
    }
    
    private void zombieCountDecrease()  //좀비 수 감소 함수
    {
        nowZombieCount--;
    }
}