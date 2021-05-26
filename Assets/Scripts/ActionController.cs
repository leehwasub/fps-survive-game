using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //습득 가능한 최대 거리

    private bool pickupActivated; //습득 가능하면 true
    private bool dissolveActivated; //고기 해체 가능할 시 true
    private bool isDissolving; //고기 해체 중에는 false

    private RaycastHit hitInfo; // 충돌체 정보 저장

    //아이템 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask layerMask;


    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private Transform tfMeatDissolveTool; //고기 해체툴

    [SerializeField]
    private string soundMeat; // 소리 재생

    private void Update()
    {
        CheckAction();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckAction();
            CanPickUp();
            CanMeat();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다.");
                inventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    IEnumerator MeatCoroutine()
    {
        //무기 교체 방지
        WeaponManager.isChangeWeapon = true;
        WeaponSway.isActivated = false;
        //교체 애니메이션
        WeaponManager.currentWeaponAnim.SetTrigger("Weapon_Out");
        PlayerController.isActivated = false;

        yield return new WaitForSeconds(0.2f);
        
        // 현재 무기 false
        WeaponManager.currentWeapon.gameObject.SetActive(false);
        // 해제 무기 true
        tfMeatDissolveTool.gameObject.SetActive(true);

       
        yield return new WaitForSeconds(2f);
        SoundManager.instance.PlaySE(soundMeat);
        yield return new WaitForSeconds(1.8f);

        Animal animal = hitInfo.transform.GetComponent<Animal>();
        inventory.AcquireItem(animal.getItem, animal.ItemNumber);

        WeaponManager.currentWeapon.gameObject.SetActive(true);
        tfMeatDissolveTool.gameObject.SetActive(false);

        PlayerController.isActivated = true;
        WeaponSway.isActivated = true;
        WeaponManager.isChangeWeapon = false;
        isDissolving = false;
    }

    private void CanMeat()
    {
        if (dissolveActivated)
        {
            Debug.Log("CanMeat()");
            if((hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal") &&  hitInfo.transform.GetComponent<Animal>().IsDead && !isDissolving)
            {
                isDissolving = true;
                InfoDisappear();
                StartCoroutine(MeatCoroutine());
            }
        }
    }


    private void CheckAction()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
            else if (hitInfo.transform.tag == "WeakAnimal" || hitInfo.transform.tag == "StrongAnimal")
            {
                MeatInfoAppear();
            }
            else
                InfoDisappear();
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void MeatInfoAppear()
    {
        if (hitInfo.transform.GetComponent<Animal>().IsDead)
        {
            dissolveActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = hitInfo.transform.GetComponent<Animal>().AnimalName + " 해체하기 " + "<color=yellow>" + "(E)" + "</color>";
        }
    }

    private void InfoDisappear()
    {
        dissolveActivated = false;
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

}
