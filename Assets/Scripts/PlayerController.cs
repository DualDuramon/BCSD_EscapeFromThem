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
        Vector3 walkVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.Translate(walkVec.normalized * walkSpeed * Time.deltaTime);
    }
}
