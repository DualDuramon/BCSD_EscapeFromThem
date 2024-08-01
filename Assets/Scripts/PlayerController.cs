﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerModel;    //플레이어 모델오브젝트
    [SerializeField] private Transform bulletPos;       //총알 발사 위치
    [SerializeField] private GameObject BulletPrefab;   //총알 프리펩
    private Rigidbody myRigid;                          //리지드바디
    private PlayerStatus myStatus;                      //플레이어 스테이터스
    private Camera myCamera;                            //카메라

    private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
        myRigid = GetComponent<Rigidbody>();
        myStatus = GetComponent<PlayerStatus>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        //if (myStatus.isDead) return;

        TryWalk();
        LookMouseCursor();
        TryGunFire();
        TryReload_Mag();
    }

    private void TryWalk()      //플레이어 이동 시도 함수
    {
        Walk();
    }

    private void Walk() //플레이어 이동 함수
    {
        float getAxisX = Input.GetAxisRaw("Horizontal");
        float getAxisZ = Input.GetAxisRaw("Vertical");

        if(getAxisX == 0 && getAxisZ == 0) {
            return;
        }

        Vector3 tempVec = (transform.position - myCamera.transform.position);

        Vector3 xVec = new Vector3(-tempVec.z, 0, tempVec.x);
        Vector3 zVec = new Vector3(tempVec.x, 0, tempVec.z);

        Vector3 walkVec = (-1) * xVec * getAxisX + zVec * getAxisZ;

        //transform.Translate(walkVec.normalized * myStatus.walkSpeed * Time.deltaTime);
        myRigid.MovePosition(transform.position + walkVec.normalized * myStatus.walkSpeed * Time.deltaTime);
    }

    private void LookMouseCursor()  //플레이어의 마우스 향하기 함수
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 mouseDir = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;
            playerModel.transform.forward = mouseDir.normalized;
        }

    }

    private void TryGunFire()  //플레이어의 발사 함수
    {
        if (Input.GetButton("Fire1") && myStatus.CanFire())
        {
            GunFire();
        }
    }   

    private void GunFire()  //사격 함수
    {
        myStatus.Decrease_bullet();
        Instantiate(BulletPrefab, bulletPos.position, bulletPos.rotation);

        //Debug.Log("총알 발사");
    }

    private void TryReload_Mag()    //재장전 시도 함수
    {
        if (Input.GetKeyDown(KeyCode.R) && myStatus.CanReloading())
        {
            StartCoroutine(ReloadingMagCoroutine());
        }
    }

    IEnumerator ReloadingMagCoroutine() // 재장전 코루틴
    {
        myStatus.isReloading = true;
        myStatus.nowBullet_reserve += myStatus.nowBullet_mag;     //전체 총알 합산 과정
        myStatus.nowBullet_mag = 0;
        Debug.Log("재장전 시작");
        
        yield return new WaitForSeconds(myStatus.reloadingTime);

        myStatus.Calculate_Bullet_Reload();
        myStatus.isReloading = false;
        Debug.Log("재장전 완료");
    }
}
