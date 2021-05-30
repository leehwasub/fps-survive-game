using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; //이름
    public GameObject goPrefab; //실제 설치될 프리팹
    public GameObject goPreviewPrefab; //미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField]
    private GameObject goBaseUI;

    [SerializeField]
    private Craft[] craftFire; // 모닥불용 탭

    private GameObject goPreview; //미리보기 프리팹을 담을 변수
    private GameObject goPrefab; //실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tfPlayer; // 플레이어 위치

    //RayCast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    public void SlotClick(int slotNumber)
    {
        goPreview = Instantiate(craftFire[slotNumber].goPreviewPrefab, tfPlayer.position + tfPlayer.forward, Quaternion.identity);
        goPrefab = craftFire[slotNumber].goPrefab;
        isPreviewActivated = true;
        goBaseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(goPrefab, hitInfo.point, Quaternion.identity);
            Destroy(goPreview);
            isActivated = false;
            isPreviewActivated = false;
            goPreview = null;
            goPrefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tfPlayer.position, tfPlayer.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 loation = hitInfo.point;
                goPreview.transform.position = loation;
            }
        }
    }


    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(goPreview);
            isActivated = false;
            isPreviewActivated = false;
            goPreview = null;
            goPrefab = null;
            goBaseUI.SetActive(false);
        }
    }

    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        isActivated = true;
        goBaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        goBaseUI.SetActive(false);
    }

}
