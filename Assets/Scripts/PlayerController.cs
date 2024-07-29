using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.0f;    //이동속도
    [SerializeField] private GameObject playerModel;
    private Camera myCamera;

    void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
    }

    void Start()
    {

    }

    void Update()
    {
        Walk();
        LookMouseCursor();
    }

    private void Walk() //플레이어 이동 함수
    {
        float getAxisX = Input.GetAxisRaw("Horizontal");
        float getAxisY = Input.GetAxisRaw("Vertical");

        Vector3 walkVec = new Vector3(getAxisX, 0, getAxisY);
        transform.Translate(walkVec.normalized * walkSpeed * Time.deltaTime);
    }

    void LookMouseCursor()  //플레이어의 마우스 향하기 함수
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 mouseDir = new Vector3(rayHit.point.x, transform.position.y, rayHit.point.z) - transform.position;
            playerModel.transform.forward = mouseDir.normalized;
        }

    }
}
