using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Kit
{
    public string kitName;
    public string kitDescription;
    public string[] needItemName;
    public int[] needItemNumber;

    public GameObject goKitPrefab;
}

public class ComputerKit : MonoBehaviour
{
    [SerializeField]
    private Kit[] kits;

    [SerializeField]
    private Transform tfItemAppear; //생성될 아이템 위치
    [SerializeField]
    private GameObject goBaseUI;

    private bool isCraft = false; //중복 실행 방지
    public bool isPowerOn = false; //전원 커졌는지

    //필요한 컴포넌트
    private Inventory inventory;
    [SerializeField]
    private ComputerToolTip tooltip;

    private AudioSource audio;
    [SerializeField] private AudioClip soundButtonClick;
    [SerializeField] private AudioClip soundBeep;
    [SerializeField] private AudioClip soundActiviated;
    [SerializeField] private AudioClip soundOutput;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventory = FindObjectOfType<Inventory>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isPowerOn && Input.GetKeyDown(KeyCode.Escape)){
            PowerOff();
        }
    }

    public void PowerOn()
    {
        GameManager.isOpenRocketComputer = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPowerOn = true;
        goBaseUI.SetActive(true);
    }

    private void PowerOff()
    {
        GameManager.isOpenRocketComputer = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPowerOn = false;
        tooltip.HideToolTip();
        goBaseUI.SetActive(false);
    }

    public void ShowToolTip(int buttonNum)
    {
        tooltip.ShowToolTip(kits[buttonNum].kitName, kits[buttonNum].kitDescription, kits[buttonNum].needItemName, kits[buttonNum].needItemNumber);
    }

    public void HideToolTip()
    {
        tooltip.HideToolTip();
    }

    private void PlaySE(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }

    public void ClickButton(int slotNumber)
    {
        PlaySE(soundButtonClick);
        if (!isCraft)
        {
            if (!CheckIngredient(slotNumber)) return; //재료 체크

            isCraft = true;
            UseIngredient(slotNumber); // 재료 사용

            StartCoroutine(CraftCoroutine(slotNumber)); // Kit 생성
        }
    }

    private void UseIngredient(int slotNumber)
    {
        for (int i = 0; i < kits[slotNumber].needItemName.Length; i++)
        {
            inventory.setItemCount(kits[slotNumber].needItemName[i], kits[slotNumber].needItemNumber[i]);
        }
    }

    private bool CheckIngredient(int slotNumber)
    {
        for (int i = 0; i < kits[slotNumber].needItemName.Length; i++)
        {
            if (inventory.GetItemCount(kits[slotNumber].needItemName[i]) < kits[slotNumber].needItemNumber[i])
            {
                PlaySE(soundBeep);
                return false;
            }
        }
        return true;
    }

    private IEnumerator CraftCoroutine(int slotNumber)
    {
        PlaySE(soundActiviated);
        yield return new WaitForSeconds(3f);
        PlaySE(soundOutput);

        Instantiate(kits[slotNumber].goKitPrefab, tfItemAppear.position, Quaternion.identity);
        isCraft = false;
    }

}
