using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName; //이름
    public Sprite craftImage; //이미지
    public string craftDesc; //설명
    public string[] craftNeedItem; //필요한아이템
    public int[] craftNeedItemCount; //필요한 아이템의 개수
    public GameObject goPrefab; //실제 설치될 프리팹
    public GameObject goPreviewPrefab; //미리보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    [SerializeField]
    private GameObject goBaseUI;

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Craft[] craftSelectedTab;

    [SerializeField]
    private Craft[] craftFire; // 모닥불용 탭
    [SerializeField]
    private Craft[] craftBuild; // 건축용 탭

    private GameObject goPreview; //미리보기 프리팹을 담을 변수
    private GameObject goPrefab; //실제 생성될 프리팹을 담을 변수

    [SerializeField]
    private Transform tfPlayer; // 플레이어 위치

    //RayCast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    //필요한 UI Slot 요소
    [SerializeField]
    private GameObject[] goSlots;
    [SerializeField]
    private Image[] imageSlot;
    [SerializeField]
    private Text[] textSlotName;
    [SerializeField]
    private Text[] textSlotDesc;

    private void Start()
    {
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craftFire);
    }

    public void TabSetting(int tabNumber)
    {
        this.tabNumber = tabNumber;
        page = 1;

        switch (tabNumber)
        {
            case 0:
                TabSlotSetting(craftFire);
                break;
            case 1:
                TabSlotSetting(craftBuild);
                break;
        }
    }

    private void ClearSlot()
    {
        for (int i = 0; i < goSlots.Length; i++)
        {
            imageSlot[i].sprite = null;
            textSlotDesc[i].text = "";
            textSlotName[i].text = "";
            goSlots[i].SetActive(false);
        }
    }

    public void RightPageSetting()
    {
        Debug.Log("Right : " + page);
        if (page < (craftSelectedTab.Length / goSlots.Length) + 1)
            page++;
        else
            page = 1;
        TabSlotSetting(craftSelectedTab);
    }

    public void LeftPageSetting()
    {
        Debug.Log("Left : " + page);
        if (page != 1)
            page--;
        else
            page = (craftSelectedTab.Length / goSlots.Length) + 1;
        TabSlotSetting(craftSelectedTab);
    }

    void TabSlotSetting(Craft[] craftTab)
    {
        ClearSlot();
        craftSelectedTab = craftTab;
        int startSlotNumber = (page - 1) * goSlots.Length;

        for (int i = startSlotNumber; i < craftSelectedTab.Length; i++)
        {
            if(i == page * goSlots.Length)
            {
                break;
            }
            goSlots[i - startSlotNumber].SetActive(true);

            imageSlot[i - startSlotNumber].sprite = craftSelectedTab[i].craftImage;
            textSlotName[i - startSlotNumber].text = craftSelectedTab[i].craftName;
            textSlotDesc[i - startSlotNumber].text = craftSelectedTab[i].craftDesc;
        }
    }



    public void SlotClick(int slotNumber)
    {
        selectedSlotNumber = slotNumber + (page - 1) * goSlots.Length;
        goPreview = Instantiate(craftSelectedTab[selectedSlotNumber].goPreviewPrefab, tfPlayer.position + tfPlayer.forward, Quaternion.identity);
        goPrefab = craftSelectedTab[selectedSlotNumber].goPrefab;
        isPreviewActivated = true;
        goBaseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (isPreviewActivated && goPreview.GetComponent<PreviewObject>().isBulidable())
        {
            Instantiate(goPrefab, goPreview.transform.position, goPreview.transform.rotation);
            Destroy(goPreview);
            isActivated = false;
            isPreviewActivated = false;
            goPreview = null;
            goPrefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tfPlayer.position, tfPlayer.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    goPreview.transform.Rotate(0, -90f, 0f);
                }else if (Input.GetKeyDown(KeyCode.E))
                {
                    goPreview.transform.Rotate(0, 90f, 0f);
                }

                location.Set(Mathf.Round(location.x), Mathf.Round(location.y / 0.1f) * 0.1f, Mathf.Round(location.z));
                goPreview.transform.position = location;
            }
        }
    }


    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(goPreview);
            isActivated = false;
            isPreviewActivated = false;
            goPreview = null;
            goPrefab = null;
            goBaseUI.SetActive(false);
        }
    }

    private void Window()
    {
        if (!isActivated)
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
        isActivated = true;
        goBaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        goBaseUI.SetActive(false);
    }

}
