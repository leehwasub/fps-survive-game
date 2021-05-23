using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool inventoryActivated;

    //필요한 컴포넌트
    [SerializeField]
    private GameObject goInventoryBase;
    [SerializeField]
    private GameObject goSlotsParent;

    //슬롯들
    private Slot[] slots;

    public bool InventoryActivated => inventoryActivated;


    void Start()
    {
        slots = goSlotsParent.GetComponentsInChildren<Slot>();
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
        if (Item.ItemType.Equpiment != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null && slots[i].item.itemName == item.itemName)
                {
                    slots[i].SetSlotCount(count);
                    return;
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
    }

}
