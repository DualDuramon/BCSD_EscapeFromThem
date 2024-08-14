using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerModel;        //플레이어 모델오브젝트
    
    [SerializeField] private Transform bulletPos;           //총알 발사 위치
    [SerializeField] private GameObject bulletPrefab;       //총알 프리펩
    [SerializeField] private ParticleSystem muzzleFlash;    //총구섬광 파티클

    [SerializeField] private GameObject grenadePrefab;      //수류탄 프리펩
    [SerializeField] private Transform grenadePos;          //수류탄 발사 위치

    //컴포넌트들
    private Animator myAnim;                            //애니메이터
    private Rigidbody myRigid;                          //리지드바디
    private Collider myCol;                             //콜라이더
    private PlayerStatus myStatus;                      //플레이어 스테이터스
    private Camera myCamera;                            //카메라

    private Coroutine currentCoroutine;

    private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
        myRigid = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        myStatus = GetComponent<PlayerStatus>();
        myAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!myStatus.isDead && !GameManager.Instance.isPause) {
            LookMouseCursor();
            TryGunFire();
            TryReload_Mag();
            TryThrowGrenade();
        }
    }

    private void FixedUpdate()
    {
        if (!myStatus.isDead && !GameManager.Instance.isPause)
        {
            TryWalk();
        }
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
            myAnim.SetInteger("Status_walk", 0);
            return;
        }
        else
        {
            myAnim.SetInteger("Status_walk", 1);
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
        playerModel.transform.forward = GetMouseCursorPos().normalized;
    }

    private Vector3 GetMouseCursorPos() //필드상에서의 마우스 좌표 반환 함수
    {
        Vector3 mouseDir = new Vector3(0, 0, 0);
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.transform.tag == "Zombie")
            {
                mouseDir = rayHit.transform.position - transform.position;
            }
            else
            {
                mouseDir = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;
            }
        }

        return mouseDir;
    }

    private void TryGunFire()  //플레이어의 발사 함수
    {
        if (Input.GetButton("Fire1") && myStatus.CanFire())
        {
            muzzleFlash.Play();
            GunFire();
        }
    }   

    private void GunFire()  //사격 함수
    {
        myStatus.Decrease_nowBullet();
        Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);
        myStatus.nowGunCoolTime = 0.0f;
    }

    private void TryThrowGrenade()  //수류탄 투척 시도 함수
    {
        if (Input.GetButtonDown("Fire2") && myStatus.CanThrowGrenade())
        {
            ThrowGrenade();
        }
    }

    private void ThrowGrenade() //수류탄 투척 함수
    {
        GameObject grenade = Instantiate(grenadePrefab, grenadePos.position, Quaternion.identity);
        grenade.transform.GetComponent<Rigidbody>().AddForce(grenadePos.forward.normalized * myStatus.throwPower, ForceMode.Impulse);
        myStatus.Calculate_Grenade(-1);
        myStatus.nowGunCoolTime = 0.0f;
    }

    private void TryReload_Mag()    //재장전 시도 함수
    {
        if (Input.GetKeyDown(KeyCode.R) && myStatus.CanReloading())
        {
            currentCoroutine = StartCoroutine(ReloadingMagCoroutine());
        }
    }

    IEnumerator ReloadingMagCoroutine() // 재장전 코루틴
    {
        myStatus.isReloading = true;
        myStatus.nowBullet_reserve += myStatus.nowBullet_mag;     //전체 총알 합산 과정
        myStatus.nowBullet_mag = 0;
        myAnim.SetFloat("ReloadSpeed", 0.5f / myStatus.reloadingTime);
        myAnim.SetInteger("Status_stg44", 3);
        Debug.Log("재장전 시작");
        
        yield return new WaitForSeconds(myStatus.reloadingTime);

        myStatus.Calculate_Bullet_Reload();
        myAnim.SetInteger("Status_stg44", 2);
        myStatus.isReloading = false;
        Debug.Log("재장전 완료");
    }

    public void Dead()  //죽음 처리 함수
    {
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        myCol.enabled = false;
        myAnim.SetInteger("Status_stg44", 4);
        myAnim.SetInteger("Status_walk", 0);

        GameManager.Instance.ActiveRetryButton();
    }

    public void ResetProperties()
    {
        myAnim.SetInteger("Status_stg44", 2);
        myAnim.SetInteger("Status_walk", 0);
        myCol.enabled = true;
        myStatus.ResetStatus();
    }
}
