using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour
{

    // 현재 장착된 CloseWeapon 형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중 ??
    protected bool isAttack;
    protected bool isSwing;

    protected RaycastHit hitInfo;
    [SerializeField]
    protected LayerMask layerMask;

    protected void TryAttack()
    {
        if (Inventory.inventoryActivated) return;
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                if (CheckObject())
                {
                    if (currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree")
                    {
                        StartCoroutine(AttackCoroutine("Chop", currentCloseWeapon.workDelayA, currentCloseWeapon.workDelayB, currentCloseWeapon.workDelay));
                        return;
                    }
                }
                //코루틴 실행
                StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackDelayA, currentCloseWeapon.attackDelayB, currentCloseWeapon.attackDelay));
            }
        }
    }


    protected IEnumerator AttackCoroutine(string swingType, float delayA, float delayB, float delayC)
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger(swingType);

        yield return new WaitForSeconds(delayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(delayB);
        isSwing = false;

        yield return new WaitForSeconds(delayC - delayA - delayB);
        isAttack = false;

    }

    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }

    public virtual void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }

}
