using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float walkSpeed = 4.0f;    //이동속도
    public float nowHp = 150.0f;    //현재 체력
    public float maxHp = 150.0f;    //최대 체력
    public bool isDead = false;     //죽음 여부

    //총 관련
    public bool isReloading = false;
    public float nowGunCoolTime = 0.0f;
    public float maxGunCoolTime = 0.3f;

    public float reloadingTime = 3.0f;

    public int nowBullet_mag = 30;
    public int maxBullet_mag = 30;
    public int nowBullet_reserve = 90;

    private void Update()
    {
        if(nowGunCoolTime < maxGunCoolTime)
        {
            nowGunCoolTime += Time.deltaTime;
        }
    }

    public void DecreaseHp(float amount)  //체력 감소 함수
    {
        nowHp -= amount;

        if(nowHp <= 0)
        {
            isDead = true;
        }
    }

    public bool CanFire()   //사격 가능 여부 체크 함수
    {
        return nowGunCoolTime >= maxGunCoolTime && nowBullet_mag > 0;
    }

    public void Decrease_bullet()   //총알 감소 함수 -> 총 발사할때 사용
    {
        if (nowBullet_mag < 0) return;

        nowGunCoolTime = 0.0f;
        nowBullet_mag--;
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
}
