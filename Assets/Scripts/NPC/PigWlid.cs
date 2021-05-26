using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigWlid : StrongAnimal
{

    protected override void Update()
    {
        base.Update();
        if (viewAngle.View() && !isDead && !isAttacking)
        {
            StopAllCoroutines();
            StartCoroutine(ChaseTargetCoroutine());
        }
    }

}
