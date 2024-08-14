using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static private SoundManager instance;
    static public SoundManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SoundManager>();
            }

            return instance;
        }
    }

    private void Start()
    {
        //BGM 재생
    }

}
