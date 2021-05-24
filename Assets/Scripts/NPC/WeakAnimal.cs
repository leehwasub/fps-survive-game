using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    public void Run(Vector3 targetPos)
    {
        applySpeed = runSpeed;
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Running", isRunning);
    }

    public override void Damage(int damage, Vector3 targetPos)
    {
        base.Damage(damage, targetPos);
        if (!isDead)
        {
            Run(targetPos);
        }
    }


}
