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
        RaycastHit[] raycasts = Physics.SphereCastAll(transform.position, explodeRange, Vector3.up, 0.0f, LayerMask.GetMask("Creature"));
        
        for (int i = 0; i < raycasts.Length; i++)
        {
            Debug.Log("수류탄 감지 : " + raycasts[i].transform.name);
            switch (raycasts[i].transform.tag)
            {
                case "Player":
                    raycasts[i].transform.GetComponent<PlayerStatus>().DecreaseHp(Damage);
                    break;
                case "Zombie":
                    raycasts[i].transform.GetComponent<ZombieController>().DecreaseHp(Damage);
                    break;
            }
        }
        Instantiate(explodeParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}