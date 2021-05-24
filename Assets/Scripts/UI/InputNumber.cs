using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activated = false;

    [SerializeField]
    private Text textPreview;
    [SerializeField]
    private Text textInput;
    [SerializeField]
    private InputField ifText;

    [SerializeField]
    private GameObject goBase;

    [SerializeField]
    private ActionController actionController;

    private void Update()
    {
        if (!activated) return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OK();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    public void Call()
    {
        goBase.SetActive(true);
        activated = true;
        ifText.text = "";
        textPreview.text = DragSlot.instance.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        activated = false;
        goBase.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        int num;
        if (textInput.text != "")
        {
            if(CheckNumber(textInput.text))
            {
                num = int.Parse(textInput.text);
                if (num > DragSlot.instance.dragSlot.itemCount)
                {
                    num = DragSlot.instance.dragSlot.itemCount;
                }
            }
            else
            {
                num = 1;
            }
        }
        else
        {
            num = int.Parse(textPreview.text);
        }

        StartCoroutine(DropItemCoroutine(num));
    }

    private IEnumerator DropItemCoroutine(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, actionController.transform.position + actionController.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f);
        }
        //현재 아이템을 들고 있고, 소지한 아이템의 모든 개수를 버릴경우 아이템 파괴
        if(int.Parse(textPreview.text) == num)
        {
            if(QuickSlotController.goHandItem != null)
            {
                Destroy(QuickSlotController.goHandItem);
            }
        }

        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
        goBase.SetActive(false);
        activated = false;
    }

    private bool CheckNumber(string argString)
    {
        char[] tempCharArray = argString.ToCharArray();
        for (int i = 0; i < tempCharArray.Length; i++)
        {
            if(tempCharArray[i] >= '0' && tempCharArray[i] <= '9')
            {
                continue;
            }
            return false;
        }
        return true;
    }

}
