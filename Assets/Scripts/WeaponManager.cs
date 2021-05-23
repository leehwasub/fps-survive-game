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
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickAxes;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듬
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickAxeDictionary = new Dictionary<string, CloseWeapon>();


    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private AxeController axeController;
    [SerializeField]
    private PickaxeController pickaxeController;

    private void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickAxes.Length; i++)
        {
            pickAxeDictionary.Add(pickAxes[i].closeWeaponName, pickAxes[i]);
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
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //무기 교체 실행 (도끼)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //무기 교체 실행 (도끼)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
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
                handController.IsActivate = false;
                break;
            case "AXE":
                axeController.IsActivate = false;
                break;
            case "PICKAXE":
                pickaxeController.IsActivate = false;
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
            handController.CloseWeaponChange(handDictionary[name]);
        }
        else if (type == "AXE")
        {
            axeController.CloseWeaponChange(axeDictionary[name]);
        }
        else if (type == "PICKAXE")
        {
            pickaxeController.CloseWeaponChange(pickAxeDictionary[name]);
        }
    }

}
