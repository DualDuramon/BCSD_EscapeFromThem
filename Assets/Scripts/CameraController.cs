using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1.0f;  //회전속도
    [SerializeField] private Transform centerPos;       //회전의 중심
    
    void Start()
    {
    
    
    }

    private void FixedUpdate()
    {
        transform.RotateAround(centerPos.transform.position, Vector3.up, Input.GetAxisRaw("CameraMovingAxis"));

    }

    void Update()
    {
    }
}
