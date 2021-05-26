﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfViewAngle : MonoBehaviour
{
    [SerializeField]
    private float viewAngle; //시야각
    [SerializeField]
    private float viewDistance; // 시야거리 (10미터)
    [SerializeField]
    private LayerMask targetMask; // 타겟 마스크

    private PlayerController player;
    private NavMeshAgent nav;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        nav = GetComponent<NavMeshAgent>();
    }

    public Vector3 getTargetPos()
    {
        return player.transform.position;
    }

    public bool View()
    {
        Collider[] target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < target.Length; i++)
        {
            Transform targetTf = target[i].transform;
            if(targetTf.name == "Player")
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);
                if(angle < viewAngle * 0.5f)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position + transform.up, direction, out hit, viewDistance))
                    {
                        if(hit.transform.name == "Player")
                        {
                            //Debug.Log("플레이어가 돼지 시야 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                            return true;
                        }
                    }
                }
            }

            if (player.IsRun)
            {
                if(CaclcPathLength(player.transform.position) <= viewDistance)
                {
                    Debug.Log("돼지가 주변에서 뛰고 있는 플레이어의 움직임을 파악했습니다.");
                    return true;
                }
            }
        }
        return false;
    }

    private float CaclcPathLength(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(targetPos, path);

        Vector3[] wayPoint = new Vector3[path.corners.Length + 2];

        wayPoint[0] = transform.position;
        wayPoint[path.corners.Length + 1] = targetPos;

        float pathLength = 0;
        for (int i = 0; i < path.corners.Length; i++)
        {
            wayPoint[i + 1] = path.corners[i]; //웨이 포인트에 경로를 넣음
            pathLength += Vector3.Distance(wayPoint[i], wayPoint[i + 1]);
        }
        return pathLength;
    }

}
