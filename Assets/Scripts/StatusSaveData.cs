using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusSaveData
{
    public int nowStageIndex = 0;           //현재 스테이지 인덱스
    public float nowHp = 150.0f;            //현재 체력
    public int nowBullet_mag = 30;          //현재 탄창 탄약
    public int nowBullet_reserve = 90;      //현재 예비 탄약
    public int nowGrenade = 2;              //현재 수류탄
    public float nowWalkSpeed = 5.0f;       //현재 이동속도
    public int totalZombieKills = 0;        //좀비 킬 수


    public void ResetData() //데이터 초기화 함수
    {
        nowStageIndex = 0;
        nowHp = 150.0f;
        nowBullet_mag = 30;
        nowBullet_reserve = 90;
        nowGrenade = 2;
        nowWalkSpeed = 5.0f;
        totalZombieKills = 0;
    }
}
