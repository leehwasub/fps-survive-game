using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp; //체력

    [SerializeField]
    private float DestroyTime;

    //작은 나뭇가지 조각들
    [SerializeField]
    private GameObject goLittleTwig;

    [SerializeField]
    private GameObject goHitEffectPrefab;

    //회전값 변수
    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    //필요한 사운드 이름
    [SerializeField]
    private string hitSound;
    [SerializeField]
    private string brokenSound;

    private void Start()
    {
        originRot = transform.rotation.eulerAngles;
        currentRot = originRot;
    }

    public void Damage(Transform playerTf)
    {
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(playerTf));

        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hitSound);

        GameObject clone = Instantiate(goHitEffectPrefab, 
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                       Quaternion.identity);
        Destroy(clone, DestroyTime);
    }

    IEnumerator HitSwayCoroutine(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;
        CheckDirection(rotationDir);

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

        wantedRot = originRot;

        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.15f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

    }

    private bool CheckThreshold()
    {
        if (Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)
            return true;
        return false;
    }

    private void CheckDirection(Vector3 rotationDir)
    {
        Debug.Log(rotationDir);

        if(rotationDir.y > 180)
        {
            if (rotationDir.y > 300)
            {
                wantedRot = new Vector3(-50f, 0f, -50f);
            }
            else if (rotationDir.y > 240)
            {
                wantedRot = new Vector3(-0f, 0f, -50f);
            }
            else
            {
                wantedRot = new Vector3(50f, 0f, -50f);
            }
        }
        else if (rotationDir.y <= 180)
        {
            if (rotationDir.y < 60)
            {
                wantedRot = new Vector3(-50f, 0f, 50f);
            }
            else if (rotationDir.y > 120)
            {
                wantedRot = new Vector3(0f, 0f, 50f);
            }
            else
            {
                wantedRot = new Vector3(50f, 0f, 50f);
            }
        }

    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(brokenSound);

        GameObject clone1 = Instantiate(goLittleTwig,
                                       gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                       Quaternion.identity);
        GameObject clone2 = Instantiate(goLittleTwig,
                                       gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),
                                       Quaternion.identity);
        Destroy(clone1, DestroyTime);
        Destroy(clone2, DestroyTime);

        Destroy(gameObject);
    }

}
