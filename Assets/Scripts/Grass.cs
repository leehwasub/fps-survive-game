using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    //풀 체력 (보통은 1)
    [SerializeField]
    private int hp;

    //이펙트 제거 시간
    [SerializeField]
    private float destroyTime;
    //폭발력 세기
    [SerializeField]
    private float force;

    //타격 효과
    [SerializeField]
    private GameObject goHitEffect;

    [SerializeField]
    private Item itemLeaf;
    [SerializeField]
    private int leafCount;
    private Inventory inventory;

    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hitSound;

    void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
        inventory = FindObjectOfType<Inventory>();
    }

    public void Damage()
    {
        hp--;
        Hit();
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hitSound);

        GameObject clone = Instantiate(goHitEffect, transform.position + Vector3.up, Quaternion.identity);
        Destroy(clone, destroyTime);
    }

    private void Destruction()
    {
        inventory.AcquireItem(itemLeaf, leafCount);

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = true;
            rigidbodies[i].AddExplosionForce(force, transform.position, 1f);
            boxColliders[i].enabled = true;
        }

        Destroy(gameObject, destroyTime);
    }

}
