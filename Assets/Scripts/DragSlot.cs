using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;

    public Image ImageItem => imageItem;

    private void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemImage)
    {
        imageItem.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = imageItem.color;
        color.a = alpha;
        imageItem.color = color;
    }

}
