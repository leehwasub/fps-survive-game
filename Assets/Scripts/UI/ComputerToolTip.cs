using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerToolTip : MonoBehaviour
{
    [SerializeField] private GameObject goBaseUI;

    [SerializeField] private Text kitName;
    [SerializeField] private Text kitDes;
    [SerializeField] private Text kitNeedItem;

    public void ShowToolTip(string kitName, string kitDes, string[] needItem, int[] needItemNumber)
    {
        goBaseUI.SetActive(true);

        this.kitName.text = kitName;
        this.kitDes.text = kitDes;

        for (int i = 0; i < needItem.Length; i++)
        {
            kitNeedItem.text += needItem[i];
            kitNeedItem.text += " x " + needItemNumber[i].ToString() + "\n";
        }
    }

    public void HideToolTip()
    {
        goBaseUI.SetActive(false);
        kitName.text = "";
        kitDes.text = "";
        kitNeedItem.text = "";
    }

}
