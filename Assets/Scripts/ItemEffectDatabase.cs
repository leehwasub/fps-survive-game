using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    HP, SP, DP, HUNGRY, THIRSTY, SATISFY
}

[Serializable]
public class ItemEffect
{
    public string itemName; // 아이템의 이름
    public PartType[] part; // 부위
    public int[] num; // 수치
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    // 필요한 컴포넌트
    [SerializeField]
    private StatusController statusController;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private SlotToolTip slotToolTip;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        slotToolTip.ShowToolTip(item, pos);
    }

    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }

    public void UseItem(Item item)
    {
        if (item.itemType == Item.ItemType.Equpiment)
        {
            //장착
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        else if (item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if(itemEffects[i].itemName == item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)
                    {
                        switch (itemEffects[i].part[j])
                        {
                            case PartType.HP:
                                statusController.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case PartType.SP:
                                statusController.IncreaseSP(itemEffects[i].num[j]);
                                break;
                            case PartType.DP:
                                statusController.IncreaseDP(itemEffects[i].num[j]);
                                break;
                            case PartType.HUNGRY:
                                statusController.IncreaseHungry(itemEffects[i].num[j]);
                                break;
                            case PartType.THIRSTY:
                                statusController.IncreaseThirsty(itemEffects[i].num[j]);
                                break;
                            case PartType.SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위를 적용시키려하고 있습니다.");
                                break;
                        }
                        Debug.Log(item.itemName + " 을 사용했습니다.");
                    }
                    return;
                }
            }
            Debug.Log("일치하는 itemName이 없습니다.");
        }
    }

}
