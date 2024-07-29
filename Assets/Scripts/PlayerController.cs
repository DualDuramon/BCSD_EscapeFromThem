using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.0f;    //이동속도
    
    void Start()
    {

    }

    void Update()
    {
        Walk();
    }

    private void Walk()
    {
        float getAxisX = Input.GetAxisRaw("Horizontal");
        float getAxisY = Input.GetAxisRaw("Vertical");

        Vector3 walkVec = new Vector3(getAxisX, 0, getAxisY);
        transform.Translate(walkVec.normalized * walkSpeed * Time.deltaTime);
    }
}
