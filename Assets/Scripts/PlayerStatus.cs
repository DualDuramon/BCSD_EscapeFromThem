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

    public bool CanFire()
    {
        return nowGunCoolTime >= maxGunCoolTime && nowBullet_mag > 0;
    }

    public void Decrease_bullet()
    {
        if (nowBullet_mag < 0) return;

        nowGunCoolTime = 0.0f;
        nowBullet_mag--;
    }

    public bool CanReloading()
    {
        return !isReloading && nowBullet_reserve > 0;
    }

    public void Calculate_Bullet_Reload()       //총알 계산 및 리로딩
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
