using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;
    public static bool isOpenInventory; // 인벤토리 활성화
    public static bool isOpenCraftManual; //건축 메뉴 활성화
    public static bool isOpenRocketComputer; //로켓 컴퓨터 활성화
    public static bool isOpenArchemyTable; //연금 테이블 창 활성화

    private void Update()
    {
        if (isOpenInventory || isOpenCraftManual || isOpenArchemyTable || isOpenRocketComputer)
        {
            CursorOn();
            canPlayerMove = false;
        }
        else
        {
            CursorOff();
            canPlayerMove = true;
        }
    }

    public void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        CursorOff();
    }

}
