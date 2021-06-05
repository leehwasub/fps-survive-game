using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject goInventoryBase;
    [SerializeField]
    private GameObject goSlotsParent;
    [SerializeField]
    private GameObject goQuickSlotParent;

    [SerializeField]
    private QuickSlotController quickSlot;

    //슬롯들
    private Slot[] slots; // 인벤토리 슬롯들
    private Slot[] quickSlots; // 퀵 슬롯들
    private bool isNotPut;
    private int slotNumber;


    void Start()
    {
        slots = goSlotsParent.GetComponentsInChildren<Slot>();
        quickSlots = goQuickSlotParent.GetComponentsInChildren<Slot>();
    }

    
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }    
    }

    private void OpenInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        goInventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        goInventoryBase.SetActive(false);
    }

    public void AcquireItem(Item item, int count = 1)
    {
        PutSlot(quickSlots, item, count);
        if (!isNotPut)
        {
            quickSlot.IsActivatedQuickSlot(slotNumber);
        }

        if (isNotPut)
        {
            PutSlot(slots, item, count);
        }
        if (isNotPut)
        {
            Debug.Log("모든 아이템공간이 꽉찼습니다");
        }
    }

    private void PutSlot(Slot[] slots, Item item, int count)
    {
        if (Item.ItemType.Equpiment != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item.itemName == item.itemName)
                {
                    slotNumber = i;
                    slots[i].SetSlotCount(count);
                    isNotPut = false;
                    return;
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }

    public int GetItemCount(string itemName)
    {
        int temp = SearchSlotItem(slots, itemName);

        return (temp != 0 ? temp : SearchSlotItem(quickSlots, itemName));
    }

    private int SearchSlotItem(Slot[] slots, string itemName)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null && itemName == slots[i].item.itemName)
            {
                return slots[i].itemCount;
            }
        }
        return 0;
    }

    public void setItemCount(string itemName, int itemCount)
    {
        if(!ItemCountAdjust(slots, itemName, itemCount))
        {
            ItemCountAdjust(quickSlots, itemName, itemCount);
        }
    }

    private bool ItemCountAdjust(Slot[] slots, string itemName, int itemCount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null && itemName == slots[i].item.itemName)
            {
                slots[i].SetSlotCount(-itemCount);
                return true;
            }
        }
        return false;
    }


}
