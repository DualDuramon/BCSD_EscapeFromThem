using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;



public class ClickingLight : MonoBehaviour
{
    enum EffectType
    {
        Clicking,
        Fire
    }

    [SerializeField] private Light myLight;
    [SerializeField] private float timer = 0;
    [SerializeField] private float RandomRange = 0.0f;
    [SerializeField] private EffectType myEffect;
    private float nowTimer = 0;

    private void Awake()
    {
        myLight = GetComponent<Light>();
        nowTimer = 0.0f;
    }

    void Update()
    {
        nowTimer += Time.deltaTime;
        if (nowTimer >= timer)
        {
            LightEffect();
            nowTimer = 0.0f;
        }
    }

    private void LightEffect()
    {
        switch (myEffect)
        {
            case EffectType.Clicking:
                myLight.enabled = !myLight.enabled;
                break;

            case EffectType.Fire:
                myLight.intensity = Random.Range(0.0f, RandomRange);                 
                break;
        }
    }
}
