using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField]
    protected string animalName;
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;
    [SerializeField]
    protected float turningSpeed;
    protected float applySpeed;

    protected Vector3 direction;

    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isDead;

    [SerializeField]
    protected float walkTime;
    [SerializeField]
    protected float waitTime;
    [SerializeField]
    protected float runTime;
    protected float currentTime;

    //필요한 컴포넌트
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rigid;
    [SerializeField]
    protected BoxCollider boxCol;
    protected AudioSource audio;
    [SerializeField]
    protected AudioClip[] soundPigNormal;
    [SerializeField]
    protected AudioClip hurtSound;
    [SerializeField]
    protected AudioClip deadSound;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    private void Update()
    {
        if (isDead) return;
        Move();
        Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
            rigid.MoveRotation(Quaternion.Euler(rotation));
        }
    }

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                ResetBehaviour();
            }
        }
    }

    protected virtual void ResetBehaviour()
    {
        applySpeed = walkSpeed;
        isRunning = false;
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }
  

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("걷기");
    }

    public virtual void Damage(int damage, Vector3 targetPos)
    {
        if (isDead) return;
        hp -= damage;

        if (hp <= 0)
        {
            Dead();
            return;
        }
        PlaySE(hurtSound);
        anim.SetTrigger("Hurt");
    }

    protected void Dead()
    {
        PlaySE(deadSound);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        int random = UnityEngine.Random.Range(0, 3); //일상 사운드 3개
        PlaySE(soundPigNormal[random]);
    }

    protected void PlaySE(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
}
