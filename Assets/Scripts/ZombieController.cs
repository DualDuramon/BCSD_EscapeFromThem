using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    //체력 관련
    [SerializeField] private float nowHp;
    [SerializeField] private float maxHp = 100.0f;
    private bool isDead;
    

    //공격관련
    [SerializeField] private float attackDamage = 70.0f;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCoolTime = 2.0f;
    [SerializeField] private float nowAttackCoolTime = 0.0f;
    [SerializeField] private bool isAttacking;

    //기본 스텟 관련
    [SerializeField] private float walkSpeed = 4.0f;

    //NavMeshAgent 관련
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask playerMask;
    private NavMeshAgent myNavAgent;

    //Animator 관련
    private Animator myAnim;

    private void Awake()
    {
        nowHp = maxHp;
        isDead = false;
        //playerTransform = FindAnyObjectByType<PlayerController>().transform;
        myNavAgent = GetComponent<NavMeshAgent>();
        myNavAgent.speed = walkSpeed;
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        TryWalk();
        TryAttack();
    }

    private void TryWalk()
    {
        if (!isAttacking)
        {
            myNavAgent.SetDestination(playerTransform.position);
        }
    }

    private void TryAttack()
    {

        if(nowAttackCoolTime < attackCoolTime)
        {
            nowAttackCoolTime += Time.deltaTime;
            return;
        }
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, attackRange, playerMask))
        {
            Debug.Log("플레이어 공격");
            myAnim.SetTrigger("Attack");
            nowAttackCoolTime = 0.0f;
            isAttacking = true;
        }
    }

    private void AttackOff()
    {
        isAttacking = false;
    }
}
