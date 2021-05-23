using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private SphereCollider col;

    [SerializeField]
    private GameObject goRock; // 일반 바위

    [SerializeField]
    private GameObject goDebris; // 깨진 바위

    [SerializeField]
    private GameObject goEffectPrefabs; // 채굴 이팩트

    //필요한 사운드 이름
    [SerializeField]
    private string stikeSound;
    [SerializeField]
    private string destroySound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(stikeSound);
        GameObject clone = Instantiate(goEffectPrefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);

        col.enabled = false;
        Destroy(goRock);

        goDebris.SetActive(true);
        Destroy(goDebris, destroyTime);
    }

}
