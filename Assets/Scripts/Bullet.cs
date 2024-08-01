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
        CheckLifeTime();
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void CheckLifeTime()
    {
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
        bulletLifeTime -= Time.deltaTime;
    }
}
