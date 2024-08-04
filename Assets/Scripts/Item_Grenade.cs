using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item_Grenade : MonoBehaviour
{
    [SerializeField] private float Damage = 100.0f;              //폭발 데미지
    [SerializeField] private float explodeRange = 2.0f;         //폭발 반경
    [SerializeField] private ParticleSystem explodeParticle;    //폭발 파티클
 
    
    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") return;    //플레이어와 충돌한건 무시
        Explode();
    }

    private void Explode()  //수류탄 폭발 함수
    {
        RaycastHit[] raycasts 
            = Physics.SphereCastAll(transform.position, explodeRange, Vector3.up, 0.0f, LayerMask.GetMask("Creature"));

        foreach (RaycastHit hitData in raycasts)
        {
            RaycastHit obstCast;
            Physics.Raycast(transform.position, hitData.transform.position - transform.position, out obstCast);
            if (obstCast.transform.tag != hitData.transform.tag) continue;

            Debug.Log("수류탄 감지 : " + hitData.transform.name);
            switch (hitData.transform.tag)
            {
                case "Player":
                    hitData.transform.GetComponent<PlayerStatus>().DecreaseHp(Damage);
                    break;
                case "Zombie":
                    hitData.transform.GetComponent<ZombieController>().DecreaseHp(Damage);
                    break;
            }
        }
        Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}