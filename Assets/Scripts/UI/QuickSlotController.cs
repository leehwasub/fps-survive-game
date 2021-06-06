using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private Slot[] quickSlots; //퀵슬롯들
    [SerializeField]
    private Image[] imgCoolTime; //퀵슬롯 쿨타임
    [SerializeField]
    private Transform tfParent;

    [SerializeField]
    private Transform tfItemPos; //아이템이 위치할 손끝
    public static GameObject goHandItem; //손에 든 아이템

    //쿨타임 내용
    [SerializeField]
    private float coolTime;
    private float currentCoolTime;
    private bool isCoolTime;

    //등장내용
    [SerializeField]
    private float appearTime;
    private float currentAppearTime;
    private bool isAppear;

    private int selectedSlot; //선택된 퀵슬록 (0~7)

    [SerializeField]
    private GameObject goSelectedImage; //선택된 퀵슬롯의 이미지
    [SerializeField]
    private WeaponManager weaponManager;
    private Animator anim;

    public bool IsCoolTime => isCoolTime;
    public Slot SelectSlot => quickSlots[selectedSlot];

    private void Start()
    {
        anim = GetComponent<Animator>();
        quickSlots = tfParent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    private void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
        AppearCalc();
    }

    private void AppearReset()
    {
        currentAppearTime = appearTime;
        isAppear = true;
        anim.SetBool("Appear", isAppear);
    }

    private void AppearCalc()
    {
        if (Inventory.inventoryActivated)
            AppearReset();
        else
        {
            if (isAppear)
            {
                currentAppearTime -= Time.deltaTime;
                if (currentAppearTime <= 0)
                {
                    isAppear = false;
                    anim.SetBool("Appear", isAppear);
                }
            }
        }
    }

    private void CoolTimeCalc()
    {
        if (isCoolTime)
        {
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < imgCoolTime.Length; i++)
            {
                imgCoolTime[i].fillAmount = currentCoolTime / coolTime;
            }
            if(currentCoolTime <= 0)
            {
                isCoolTime = false;
            }
        }
    }

    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCoolTime = true;
    }

    private void TryInputNumber()
    {
        if (!isCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeSlot(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeSlot(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeSlot(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeSlot(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChangeSlot(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChangeSlot(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ChangeSlot(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ChangeSlot(7);
            }
        }
    }

    public void IsActivatedQuickSlot(int num)
    {
        if(selectedSlot == num)
        {
            ExecuteQuickSlot();
        }
        else if(DragSlot.instance != null)
        {
            if(DragSlot.instance.dragSlot.QuickSlotNumber == selectedSlot)
            {
                ExecuteQuickSlot();
            }
        }
    }

    private void ChangeSlot(int num)
    {
        SelectedSlot(num);
        ExecuteQuickSlot();
    }

    private void SelectedSlot(int num)
    {
        //선택된 슬롯
        selectedSlot = num;
        //선택된 슬롯으로 이미지 이동
        goSelectedImage.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void ExecuteQuickSlot()
    {
        CoolTimeReset();
        AppearReset();

        if (quickSlots[selectedSlot].item != null)
        {
            if(quickSlots[selectedSlot].item.itemType == Item.ItemType.Equpiment)
            {
                StartCoroutine(weaponManager.ChangeWeaponCoroutine(quickSlots[selectedSlot].item.weaponType, quickSlots[selectedSlot].item.itemName));
            }
            else if (quickSlots[selectedSlot].item.itemType == Item.ItemType.Used || quickSlots[selectedSlot].item.itemType == Item.ItemType.Kit)
            {
                ChangeHand(quickSlots[selectedSlot].item);
            }
            else
            {
                ChangeHand();
            }
        }
        else
        {
            ChangeHand();
        }
    }

    private void ChangeHand(Item item = null)
    {
        StartCoroutine(weaponManager.ChangeWeaponCoroutine("HAND", "맨손"));
        if(item != null)
        {
            StartCoroutine(HandItemCoroutine(item));
        }
    }

    IEnumerator HandItemCoroutine(Item item)
    {
        HandController.isActivate = false;
        yield return new WaitUntil(() => HandController.isActivate);

        if(item.itemType == Item.ItemType.Kit)
        {
            HandController.currentKit = item;
        }

        goHandItem = Instantiate(quickSlots[selectedSlot].item.itemPrefab, tfItemPos.position, tfItemPos.rotation);
        goHandItem.GetComponent<Rigidbody>().isKinematic = true;
        goHandItem.GetComponent<BoxCollider>().enabled = false;
        goHandItem.tag = "Untagged";
        goHandItem.layer = 9;
        goHandItem.transform.SetParent(tfItemPos);
    }

    public void DecreaseSelectedItem()
    {
        CoolTimeReset();
        AppearReset();
        quickSlots[selectedSlot].SetSlotCount(-1);
        if(quickSlots[selectedSlot].itemCount <= 0)
        {
            Destroy(goHandItem);
        }
    }


}
