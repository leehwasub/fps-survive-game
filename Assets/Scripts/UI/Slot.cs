using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; //획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage;

    //필요한 컴포넌트
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private GameObject goCountImage;

    private ItemEffectDatabase itemEffectDatabase;

    void Start()
    {
        itemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                //소모
                itemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                {
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag()");
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop()");
        //애초에 A가 비었을때
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempItemCount = itemCount;
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(tempItem != null)
        {
            //A->B A아이템있고 B아이템 있는경우
            DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
        }
        else
        {
            //A->B A아이템있고 B가 빈경우
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            itemEffectDatabase.ShowToolTip(item, transform.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffectDatabase.HideToolTip();
    }

}
