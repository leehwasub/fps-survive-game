using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    //깎일 나무 조각들
    [SerializeField]
    private GameObject[] goTreePieces;
    [SerializeField]
    private GameObject goTreeCenter;

    [SerializeField]
    private GameObject goLogPrefabs;

    //랜덤으로 쓰러질때 가해질 힘의 세기
    [SerializeField]
    private float force;
    [SerializeField]
    private GameObject goChildTree;

    //부모 트리 파괴되면 캡슐 콜라이더 제거
    [SerializeField]
    private CapsuleCollider parentCol;

    //자식 트리가 쓰러질때 필요한 컴포넌트 활성화 및 중력 활성화
    [SerializeField]
    private CapsuleCollider childCol;
    [SerializeField]
    private Rigidbody childRigid;

    //도끼질 효과
    [SerializeField]
    private GameObject goHitEffectPrefab;

    //파편 제거 시간
    [SerializeField]
    private float debrisDestoryTime;

    //나무 제거 시간
    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private string chopSound;
    [SerializeField]
    private string fallDownSound;
    [SerializeField]
    private string logChangeSound;

    public void Chop(Vector3 pos, float angleY)
    {
        Hit(pos);
        Debug.Log(angleY);
        AngleCalc(angleY);
        if (CheckTreePiece())
        {
            return;
        }

        FallDownTree();
    }


    //적중 이펙트
    private void AngleCalc(float angleY)
    {
        for(int i = 0; i < 350; i += 70)
        {
            if(i <= angleY && angleY <= i + 70)
            {
                int index = i / 70;
                DestroyPiece((index + 2) % 5);
                break;
            }
        }
    }

    private void DestroyPiece(int num)
    {
        if(goTreePieces[num].gameObject != null)
        {
            GameObject clone = Instantiate(goHitEffectPrefab, goTreePieces[num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestoryTime);
            Destroy(goTreePieces[num].gameObject);
        }
    }

    private void Hit(Vector3 pos)
    {
        SoundManager.instance.PlaySE(chopSound);

        GameObject clone = Instantiate(goHitEffectPrefab, pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestoryTime);
    }

    private bool CheckTreePiece()
    {
        for (int i = 0; i < goTreePieces.Length; i++)
        {
            if(goTreePieces[i].gameObject != null)
            {
                return true;
            }
        }
        return false;
    }

    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(fallDownSound);
        Destroy(goTreeCenter);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(UnityEngine.Random.Range(-force, force), 0f, UnityEngine.Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }

    private IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChangeSound);

        for(int i = 0; i < 3; i++)
        {
            Instantiate(goLogPrefabs, goChildTree.transform.position + (goChildTree.transform.up * 3f * (i + 1)), Quaternion.LookRotation(goChildTree.transform.up));
        }
        Destroy(goChildTree.gameObject);
    }

    public Vector3 GetTreeCenterPosition()
    {
        return goTreeCenter.transform.position;
    }

}
