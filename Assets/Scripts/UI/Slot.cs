using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage;

    //필요한 컴포넌트
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private GameObject goCountImage;

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void AddItem(Item item, int count = 1)
    {
        this.item = item;
        itemCount = count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equpiment)
        {
            goCountImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            goCountImage.SetActive(false);
        }
        SetColor(1);
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        textCount.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        textCount.text = "0";
        goCountImage.SetActive(false);
    }

}
