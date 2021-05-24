using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{

    [SerializeField]
    protected float chaseTime; //총 추격시간
    protected float currentChaseTime; //계산
    [SerializeField]
    protected float chaseDelayTime; //추격 딜레이

    public void Chase(Vector3 targetPos)
    {
        isChasing = true;
        destination = targetPos;
        nav.speed = runSpeed;
        isRunning = true;
        anim.SetBool("Running", isRunning);
        nav.SetDestination(destination);
    }

    public override void Damage(int damage, Vector3 targetPos)
    {
        base.Damage(damage, targetPos);
        if (!isDead)
        {
            Chase(targetPos);
        }
    }

}
