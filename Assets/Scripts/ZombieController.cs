using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    //체력 관련
    [SerializeField] private float nowHp;                   //현재 체력
    [SerializeField] private float maxHp = 100.0f;          //최대 체력
    [SerializeField] private bool isDead;                   //사망 여부
    

    //공격관련
    [SerializeField] private float attackDamage = 70.0f;        //데미지
    [SerializeField] private float attackRange = 2.0f;          //공격범위
    [SerializeField] private float attackCoolTime = 2.0f;       //최대 공격 쿨타임
    [SerializeField] private float nowAttackCoolTime = 0.0f;    //현재 공격 쿨타임
    [SerializeField] private bool isAttacking;                  //공격 중 여부

    //기본 스텟 관련
    [SerializeField] private float walkSpeed = 4.0f;            //걷기 속도

    //NavMeshAgent 관련
    [SerializeField] private Transform playerTransform;         //플레이어 위치
    [SerializeField] private LayerMask playerMask;              //플레이어 레이어마스크
    private NavMeshAgent myNavAgent;

    //메시 그리기 여부 관련
    [SerializeField] private float visibleDistance = 5.0f;      //메시 그려지는 최소 거리
    [SerializeField] private float visibleDotValue = -0.766f;   //cos(-140도) = -0.766f
    private SkinnedMeshRenderer meshRenderer;                   //메시 렌더러
    

    //그외
    private Animator myAnim;                                    //애니메이터
    private Collider myCollider;                                //콜라이더
    [SerializeField] private SpawnManager spawnManager;         //스폰 매니저 -> 오브젝트 풀링때문에 자신의 부모가 됨.

    private void Awake()
    {
        //컴포넌트 불러오기
        myNavAgent = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerTransform = GameObject.Find("PlayerBody").transform;
        spawnManager = gameObject.GetComponentInParent<SpawnManager>();
        //ResetProperties();
    }

    private void OnEnable()
    {
        ResetProperties();
    }

    private void ResetProperties()  //컴포넌트 프로퍼티 초기화 함수
    {
        nowHp = maxHp;
        isDead = false;
        myAnim.SetBool("isDead", false);
        myAnim.SetBool("isWalk", true);
        myNavAgent.enabled = true;
        myCollider.enabled = true;
        meshRenderer.enabled = true;
        myNavAgent.SetDestination(playerTransform.position);
        myNavAgent.speed = walkSpeed;
    }

    private void Update()
    {
        if (!isDead && GameManager.Instance.didPlayerGetBonus)
        {
            TryWalk();
            TryAttack();
        }
    }
    
    private void FixedUpdate()
    {
        if(!isDead && GameManager.Instance.didPlayerGetBonus)
        {
            HideMyMesh();
        }
    }

    private void TryWalk()  //걷기 함수
    {
        if (!isAttacking)
        {
            myNavAgent.SetDestination(playerTransform.position);
            myAnim.SetBool("isWalk", true);
        }
    }

    private void TryAttack()    //공격 시도 함수
    {
        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red);
        
        if (nowAttackCoolTime < attackCoolTime)
        {
            nowAttackCoolTime += Time.deltaTime;
            return;
        }
        else
        {
            Attack();
        }
    }

    private void Attack()   //공격 함수
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            transform.LookAt(playerTransform.position);
        else return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, attackRange, playerMask))
        {
            Debug.Log("플레이어 공격");
            hitInfo.transform.GetComponent<PlayerStatus>().DecreaseHp(attackDamage);
            myAnim.SetTrigger("Attack");
            nowAttackCoolTime = 0.0f;
            isAttacking = true;
        }
    }

    public void DecreaseHp(float amount)
    {
        if (isDead) return;

        nowHp -= amount;
        if (nowHp <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()  //사망 처리 코루틴
    {
        isDead = true;
        myNavAgent.ResetPath();
        myAnim.SetBool("isDead", true);
        myAnim.SetBool("isWalk", false);
        myNavAgent.enabled = false;
        myCollider.enabled = false;
        meshRenderer.enabled = true;

        yield return new WaitForSeconds(5.0f);  //5초 대기 후 시체 삭제 및 후보정
        spawnManager.ZombieDead(gameObject);
    }

    private void AttackOff()    //공격 끝나면 공격 풀게함. 애니메이션에서 이벤트 호출
    {
        isAttacking = false;
    }

    private void HideMyMesh() //좀비 메시 숨기기,밝히기 함수
    {
        float dotValue 
            = Vector3.Dot(playerTransform.forward.normalized, 
            (playerTransform.position - transform.position).normalized);
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        //Debug.Log(dotValue);
        if (dotValue < visibleDotValue || distance < visibleDistance)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
