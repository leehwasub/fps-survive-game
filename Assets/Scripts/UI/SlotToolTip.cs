using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    //필요한 컴포넌트
    [SerializeField]
    private GameObject goBase;

    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemDesc;
    [SerializeField]
    private Text itemHowToUse;

    public void ShowToolTip(Item item, Vector3 pos)
    {
        goBase.SetActive(true);
        pos += new Vector3(goBase.GetComponent<RectTransform>().rect.width / 2f, -goBase.GetComponent<RectTransform>().rect.height / 2f, 0f);
        goBase.transform.position = pos;
        itemName.text = item.itemName;
        itemDesc.text = item.itemDesc;

        if(item.itemType == Item.ItemType.Equpiment)
        {
            itemHowToUse.text = "우클릭 - 장착";
        }
        else if(item.itemType == Item.ItemType.Used)
        {
            itemHowToUse.text = "우클릭 - 먹기";
        }
        else
        {
            itemHowToUse.text = "";
        }
    }

    public void HideToolTip()
    {
        goBase.SetActive(false);
    }

}
