﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 너클이나 맨손을 구분

    // 웨폰 유형
    public bool isHand;
    public bool isAxe;
    public bool isPickAxe;

    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 딜레이
    public float attackDelayB; // 공격 비활성화 딜레이

    public float workDelay; // 공격 딜레이
    public float workDelayA; // 공격 활성화 딜레이
    public float workDelayB; // 공격 비활성화 딜레이

    public Animator anim; //애니메이션


}
