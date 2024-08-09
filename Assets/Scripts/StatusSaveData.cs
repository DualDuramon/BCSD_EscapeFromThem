using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusSaveData
{
    public float nowHp = 150.0f;        //현재 체력
    public int nowBullet_mag = 30;      //현재 탄창 탄약
    public int nowBullet_reserve = 90;  //현재 예비 탄약
    public int nowGrenade = 1;          //현재 수류탄
    public float nowWalkSpeed = 5.0f;       //현재 이동속도
    public int zombieKills = 0;         //좀비 킬 수
}
