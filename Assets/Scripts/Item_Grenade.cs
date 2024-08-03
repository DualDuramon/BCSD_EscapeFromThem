using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item_Grenade : MonoBehaviour
{
    [SerializeField] private float Damage = 100.0f;         //폭발 데미지
    [SerializeField] private float explodeRange = 2.0f;     //폭발 반경
    [SerializeField] private LayerMask targetMask;          //수류탄 대상 레이어 마스크

    private void Start()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explodeRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") return;    //플레이어와 충돌한건 무시
        Explode();
    }

    private void Explode()  //수류탄 폭발 함수
    { 
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange,targetMask);
        for(int i = 0; i < colliders.Length; i++)
        {
            Debug.Log("수류탄 감지 : " + colliders[i].transform.name);
            switch (colliders[i].transform.tag)
            {
                case "Player":
                    colliders[i].GetComponent<PlayerStatus>().DecreaseHp(Damage);
                    break;
                case "Zombie":
                    colliders[i].GetComponent<ZombieController>().DecreaseHp(Damage);
                    break;
            }
        }
        Destroy(gameObject);
    }
}