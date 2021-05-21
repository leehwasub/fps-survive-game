using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //상태변수
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    //앉았을때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //민감도
    [SerializeField]
    private float lookSensitivity; //카메라 민감도

    //카메라 한개
    [SerializeField]
    private float cameraRoationLimit; // 카메라 각도 제한
    private float currentCameraRoationX = 0;


    [SerializeField]
    private Camera myCamera;
    private Rigidbody rigidbody;
    private GunController gunController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = myCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        gunController = FindObjectOfType<GunController>();
    }

    
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRoation();
        CharacterRotation();
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else if (!isCrouch)
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float posY = myCamera.transform.localPosition.y;
        int count = 0;
        while(count < 15)
        {
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            myCamera.transform.localPosition = new Vector3(0f, posY, 0f);
            count++;
            yield return null;
        }
        myCamera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            Jump();
        }
    }

    private void IsGround()
    {
        //y크기 절반만큼 레이저 발사
        //대각선에서 오차상쇄를 위해 0.1f
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void Jump()
    {
        if (isCrouch) Crouch();
        rigidbody.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        if (isCrouch) Crouch();

        gunController.CancelFindSight();

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        // 크기를 1로만들어 속도를 계산하기 쉽도록 하기위함
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed * Time.deltaTime;
        // (1, 0, 0) + (0, 0, 1) = (1, 0, 1) => (0.5, 0, 0.5)

        rigidbody.MovePosition(transform.position + velocity);
    }

    private void CameraRoation()
    {
        //상하 카메라 회전
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float camRotationX = xRotation * lookSensitivity;
        currentCameraRoationX = Mathf.Clamp(currentCameraRoationX - camRotationX, -cameraRoationLimit, cameraRoationLimit);

        myCamera.transform.localEulerAngles = new Vector3(currentCameraRoationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRoationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(characterRoationY));
    }


}
