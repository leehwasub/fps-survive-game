using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ArchemyItem
{
    public string itemName;
    public string itemDesc;
    public Sprite itemImage;

    public float itemCraftingTime; //포션 제조에 걸리는 시간

    public GameObject goItemPrefab;
}

public class ArchemyTable : MonoBehaviour
{

    private bool isOpen;
    private bool isCrafting;

    [SerializeField] private ArchemyItem[] archemyItems; //제작할 수 있는 연금 아이템 리스트
    private Queue<ArchemyItem> archemyItemQueue = new Queue<ArchemyItem>(); //연금 아이템 제작 대기열(큐)
    private ArchemyItem currentCraftingItem; //현재 제작중인 연금 아이템

    private float craftingTime; //제작 시간
    private float currentCraftingTime; //실제 계산

    [SerializeField] private Slider sliderGauge; //
    [SerializeField] private Transform tfBaseUI; //베이스 UI
    [SerializeField] private Transform tfPointAppearPos; //포션나올 위치
    [SerializeField] private GameObject goLiquid; // 동작시키면 액체 등장
    [SerializeField] private Image[] imageCraftingItems; // 대기열 슬롯에 있는 아이템 이미지들

    public bool IsOpen => isOpen;

    private void Update()
    {
        if (!IsFinish())
        {
            Crafting();
        }

        Debug.Log(isCrafting);
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }

    private bool IsFinish()
    {
        if(archemyItemQueue.Count == 0 && !isCrafting){
            goLiquid.SetActive(false);
            sliderGauge.gameObject.SetActive(false);
            return true;
        }
        else
        {
            goLiquid.SetActive(true);
            sliderGauge.gameObject.SetActive(true);
            return false;
        }
    }

    private void Crafting()
    {
        if (!isCrafting && archemyItemQueue.Count != 0)
        {
            DequeueItem();
        }

        if (isCrafting)
        {
            currentCraftingTime += Time.deltaTime;
            sliderGauge.value = currentCraftingTime;

            if(currentCraftingTime >= craftingTime)
            {
                ProudctionComplete();
            }
        }
    }

    private void DequeueItem()
    {
        isCrafting = true;
        currentCraftingItem = archemyItemQueue.Dequeue();

        craftingTime = currentCraftingItem.itemCraftingTime;
        currentCraftingTime = 0;
        sliderGauge.maxValue = craftingTime;

        CraftingImageChange();
    }

    private void CraftingImageChange()
    {
        imageCraftingItems[0].gameObject.SetActive(true);

        //위에서 Dequeue를 했으므로 Cound에 1을 더함
        for (int i = 0; i < archemyItemQueue.Count + 1; i++)
        {
            imageCraftingItems[i].sprite = imageCraftingItems[i + 1].sprite;
            if(i + 1 == archemyItemQueue.Count + 1)
            {
                imageCraftingItems[i + 1].gameObject.SetActive(false);
            }
        }
    }

    public void Window()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        isOpen = true;
        GameManager.isOpenArchemyTable = true;
        tfBaseUI.localScale = new Vector3(1f, 1f, 1f);
    }

    private void CloseWindow()
    {
        isOpen = false;
        GameManager.isOpenArchemyTable = false;
        tfBaseUI.localScale = new Vector3(0f, 0f, 0f);
    }

    public void ButtonClick(int buttonNum)
    {
        if(archemyItemQueue.Count < 3)
        {
            archemyItemQueue.Enqueue(archemyItems[buttonNum]);

            imageCraftingItems[archemyItemQueue.Count].gameObject.SetActive(true);
            imageCraftingItems[archemyItemQueue.Count].sprite = archemyItems[buttonNum].itemImage;
        }
    }

    private void ProudctionComplete()
    {
        isCrafting = false;
        imageCraftingItems[0].gameObject.SetActive(false);

        Instantiate(currentCraftingItem.goItemPrefab, tfPointAppearPos.position, Quaternion.identity);
    }

}
