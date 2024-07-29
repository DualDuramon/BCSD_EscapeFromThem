using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        float getAxisZ = Input.GetAxisRaw("Vertical");


        //Vector3 walkVec = new Vector3(getAxisX, 0, getAxisZ);
        Vector3 tempVec = (transform.position - myCamera.transform.position);

        Vector3 xVec = new Vector3(-tempVec.z, 0, tempVec.x);
        Vector3 zVec = new Vector3(tempVec.x, 0, tempVec.z);

        Vector3 walkVec = (-1) * xVec * getAxisX + zVec * getAxisZ;

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
