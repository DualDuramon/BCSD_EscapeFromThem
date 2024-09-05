using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10.0f; //총알 속도
    [SerializeField] private float bulletDamage = 25.0f;
    [SerializeField] private float bulletLifeTime = 3.0f; //총알 생존시간

    void Update()
    {
        Destroy(gameObject, bulletLifeTime);
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            other.gameObject.GetComponent<ZombieController>().DecreaseHp(bulletDamage);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            collision.gameObject.GetComponent<ZombieController>().DecreaseHp(bulletDamage);
        }
        Destroy(gameObject);
    }
}
