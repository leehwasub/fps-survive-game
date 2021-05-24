using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    public static bool isActivate = false;

    [SerializeField]
    private QuickSlotController quickSlot;

    private void Update()
    {
        if (isActivate && !Inventory.inventoryActivated)
        {
            if(QuickSlotController.goHandItem == null)
            {
                TryAttack();
            }
            else
            {
                TryEating();
            }
        }
    }

    private void TryEating()
    {
        if (Input.GetButtonDown("Fire1") && !quickSlot.IsCoolTime)
        {
            currentCloseWeapon.anim.SetTrigger("Eat");
            quickSlot.EatItem();
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
