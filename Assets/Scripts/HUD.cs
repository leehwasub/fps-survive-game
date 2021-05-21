using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GunController gunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요없으면 HUD 비활성화
    [SerializeField]
    private GameObject goBulletHUD;

    // 총알 갯수 텍스트에 반영
    [SerializeField]
    private Text[] textBullet;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = gunController.Gun;
        textBullet[0].text = currentGun.carryBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.currentBulletCount.ToString();
    }

}
