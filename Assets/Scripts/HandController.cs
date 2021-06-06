using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    public static bool isActivate = false;
    public static Item currentKit; // 설치하려는 킷 (연금 테이블)

    private bool isPreview = false;

    private GameObject goPreview; // 설치할 키트 프리뷰
    private Vector3 previewPos; // 설치할 키트 위지
    [SerializeField]
    private float rangeAdd; // 건축시 추가 사정거리

    [SerializeField]
    private QuickSlotController quickSlot;

    private void Update()
    {
        if (isActivate && !Inventory.inventoryActivated)
        {
            if(currentKit == null)
            {
                if (QuickSlotController.goHandItem == null)
                {
                    TryAttack();
                }
                else
                {
                    TryEating();
                }
            }
            else
            {
                if (!isPreview)
                {
                    InstallPreviewKit();
                }
                PreviewPositionUpdate();
                Build();
            }
        }
    }

    private void Build()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (goPreview.GetComponent<PreviewObject>().isBulidable())
            {
                quickSlot.DecreaseSelectedItem(); //슬롯 아이템 개수 -1
                GameObject temp = Instantiate(currentKit.kitPrefab, previewPos, Quaternion.identity);
                temp.name = currentKit.itemName;
                Destroy(goPreview);
                currentKit = null;
                isPreview = false;
            }
        }
    }

    public void Cancel()
    {
        Destroy(goPreview);
        currentKit = null;
        isPreview = false;
    }

    private void InstallPreviewKit()
    {
        isPreview = true;
        goPreview = Instantiate(currentKit.kitPreviewPrefab, transform.position, Quaternion.identity);
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range + rangeAdd, layerMask))
        {
            previewPos = hitInfo.point;
            goPreview.transform.position = previewPos;
        }
    }

    private void TryEating()
    {
        if (Input.GetButtonDown("Fire1") && !quickSlot.IsCoolTime)
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlot.DecreaseSelectedItem();
        }
    }


    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                // 충돌 됨
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        base.CloseWeaponChange(closeWeapon);
        isActivate = true;
    }

}
