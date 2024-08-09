using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float walkSpeed = 10.0f;    //이동속도
    [SerializeField] private float nowHp = 150.0f;    //현재 체력
    [SerializeField] private float maxHp = 150.0f;    //최대 체력
    public bool isDead = false;     //죽음 여부

    //총기 공격 관련
    public bool isReloading = false;
    public float nowGunCoolTime = 0.0f;
    public float maxGunCoolTime = 0.3f;

    public float reloadingTime = 3.0f;

    public int nowBullet_mag = 30;
    public int maxBullet_mag = 30;
    public int nowBullet_reserve = 90;

    //아이템 관련
    public int nowGrenade = 1;      //현재 보유 수류탄
    public int maxGrenade = 5;      //최대 수류탄 개수
    public float throwPower = 5.0f;    //수류탄 던지기 힘


    private void Awake()
    {

    }

    private void Update()
    {
        if(nowGunCoolTime < maxGunCoolTime)
        {
            nowGunCoolTime += Time.deltaTime;
        }
    }

    public void ResetStatus()   //스테이터스 초기화 함수 default
    {
        walkSpeed = 5.0f;
        nowHp = maxHp;
        isDead = false;
        isReloading = false;
        nowGunCoolTime = 0.0f;
        nowBullet_mag = maxBullet_mag;
        nowGrenade = 1;
        throwPower = 5.0f;
    }

    public void ResetStatus(ref StatusSaveData saveData)    //스테이터스 초기화 함수
    {
        walkSpeed = saveData.nowWalkSpeed;
        nowHp = saveData.nowHp;
        nowBullet_mag = saveData.nowBullet_mag;
        nowBullet_reserve = saveData.nowBullet_reserve;
        nowGrenade = saveData.nowGrenade;
        isDead = false;
        isReloading = false;
        nowGunCoolTime = 0.0f;
    }

    public void MakeNowHpMax()  //현재 HP를 최대로 만들기 함수
    {
        nowHp = maxHp;
    }

    public void DecreaseHp(float amount)  //체력 감소 함수
    {
        nowHp -= amount;

        if(nowHp <= 0)
        {
            Dead();
        }
    }

    public void Dead()  //사망 시 스테이터스 처리 함수
    {
        isDead = true;
        transform.GetComponent<PlayerController>().Dead();  //플레이어 사망 처리
    }

    public bool CanFire()   //사격 가능 여부 체크 함수
    {
        return nowGunCoolTime >= maxGunCoolTime && nowBullet_mag > 0;
    }

    public void Increase_bullet(int amount)     //보유 총알 증가 함수
    {
        nowBullet_reserve += amount;
    }

    public void Decrease_bullet()   //총알 감소 함수 -> 총 발사할때 사용
    {
        //if (nowBullet_mag < 0) return;
        nowBullet_mag--;
        if (nowBullet_mag < 0) nowBullet_mag = 0;
    }

    public bool CanThrowGrenade()   //수류탄 투척 가능 여부 체크 함수
    {
        return nowGrenade > 0 && !isReloading && nowGunCoolTime >= maxGunCoolTime;
    }

    public void IncreaseGrenade(int amount) //보유 수류탄 증가 함수
    {
        if(nowGrenade + amount > maxGrenade)
        {
            nowGrenade = maxGrenade;
        }
        else
        {
            nowGrenade += amount;
        }
    }
    
    public void DecreaseGrenade()   //보유 수류탄 감소 함수
    {
        nowGrenade--;
    }
    
    public bool CanReloading()  //재장전 가능 여부 체크 함수
    {
        return !isReloading &&
            nowBullet_reserve > 0 &&
            nowBullet_mag != maxBullet_mag;
    }

    public void Calculate_Bullet_Reload()       //재장전 시 총알 계산 함수
    {        
        if(nowBullet_reserve < maxBullet_mag)
        {
            nowBullet_mag = nowBullet_reserve;
            nowBullet_reserve = 0;
        }
        else
        {
            nowBullet_mag = maxBullet_mag;
            nowBullet_reserve -= maxBullet_mag;
        }
    }

    public void SaveMyStatus(ref StatusSaveData to) //스테이터스 저장 함수
    {
        to.nowHp = nowHp;
        to.nowBullet_mag = nowBullet_mag;
        to.nowBullet_reserve = nowBullet_reserve;
        to.nowGrenade = nowGrenade;
        to.nowWalkSpeed = walkSpeed;
    }
}
