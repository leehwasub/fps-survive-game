﻿using System;
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
        goInventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
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


}
