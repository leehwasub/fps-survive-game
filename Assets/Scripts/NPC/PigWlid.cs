using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWlid : StrongAnimal
{

    protected override void Update()
    {
        base.Update();
        if (viewAngle.View() && !isDead)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

    IEnumerator ChaseTargetCoroutine()
    {
        currentChaseTime = 0;

        while (currentChaseTime < chaseTime)
        {
            Chase(viewAngle.getTargetPos());
            yield return new WaitForSeconds(chaseDelayTime);
            currentChaseTime += chaseDelayTime;
        }

        isRunning = false;
        isChasing = false;
        anim.SetBool("Running", isRunning);
        nav.ResetPath();
    }


}
