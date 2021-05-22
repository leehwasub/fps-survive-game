using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션.
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //현재 무기의 타입
    [SerializeField]
    private string currentWeaponType;

    //무기 교체 딜레이, 무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류들 전부 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듬
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;

    private void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    private void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //무기 교체 실행 (서브 머신건)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //무기 교체 실행 (맨손)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                gunController.CancelFindSight();
                gunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            gunController.GunChange(gunDictionary[name]);
        }
        else if(type == "HAND")
        {
            handController.HandChange(handDictionary[name]);
        }
    }

}
