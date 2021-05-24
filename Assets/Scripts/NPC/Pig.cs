using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : WeakAnimal
{
    protected override void ResetBehaviour()
    {
        base.ResetBehaviour();
        RandomAction();
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }


    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }

    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번거림");
    }

    private void RandomAction()
    {
        RandomSound();
        int random = UnityEngine.Random.Range(0, 4);

        if (random == 0)
        {
            Wait();
        }
        else if (random == 1)
        {
            Eat();
        }
        else if (random == 2)
        {
            Peek();
        }
        else if (random == 3)
        {
            TryWalk();
        }
    }


}
