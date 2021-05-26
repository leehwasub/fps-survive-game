using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAnimal : Animal
{
    [SerializeField]
    protected int attackDamage;
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected LayerMask targetMask;

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

    protected IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < chaseTime)
        {
            Chase(viewAngle.getTargetPos());
            //충분히 가까이 있고
            if (Vector3.Distance(transform.position, viewAngle.getTargetPos()) <= 3f)
            {
                if (viewAngle.View()) //눈 앞에 있을 경우
                {
                    Debug.Log("공격시도");
                    StartCoroutine(AttackCoroutine());
                }
            }
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isRunning = false;
        isChasing = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nav.ResetPath();
        currentChaseTime = chaseTime;
        yield return new WaitForSeconds(0.5f);
        transform.LookAt(viewAngle.getTargetPos());
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        RaycastHit hit;
        //눈높이에서 전방으로 쏘자 
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 3, targetMask))
        {
            Debug.Log("플레이어 적중!");
            playerStatus.DecreaseHP(attackDamage);
        }
        else
        {
            Debug.Log("플레이어 빗나감!");
        }
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
        StartCoroutine(ChaseTargetCoroutine());
    }


}
