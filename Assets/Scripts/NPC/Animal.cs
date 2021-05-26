using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController playerStatus;

    [SerializeField]
    protected string animalName;
    [SerializeField]
    protected int hp;
    [SerializeField]
    private Item itemPrefabs; //아이템 
    [SerializeField]
    protected int itemNumber; // 아이템의 획득 개수
    [SerializeField]
    protected float walkSpeed;
    [SerializeField]
    protected float runSpeed;

    protected Vector3 destination;

    protected bool isAction;
    protected bool isWalking;
    protected bool isRunning;
    protected bool isChasing;
    protected bool isAttacking;
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
    protected NavMeshAgent nav;
    protected FieldOfViewAngle viewAngle;
    [SerializeField]
    protected AudioClip[] soundPigNormal;
    [SerializeField]
    protected AudioClip hurtSound;
    [SerializeField]
    protected AudioClip deadSound;

    public string AnimalName => animalName;
    public bool IsDead => isDead;
    public Item getItem
    {
        get {
            gameObject.tag = "Untagged";
            Destroy(gameObject, 3f);
            return itemPrefabs;
        }
    }
    public int ItemNumber => itemNumber;

    private void Start()
    {
        playerStatus = FindObjectOfType<StatusController>();
        viewAngle = GetComponent<FieldOfViewAngle>();
        nav = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    protected virtual void Update()
    {
        if (isDead) return;
        Move();
        //Rotation();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            //rigid.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + destination * 5f);
        }
    }

    //protected void Rotation()
    //{
    //    if (isWalking || isRunning)
    //    {
    //        Vector3 rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
    //        rigid.MoveRotation(Quaternion.Euler(rotation));
    //    }
    //}

    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && isAttacking)
            {
                ResetBehaviour();
            }
        }
    }

    protected virtual void ResetBehaviour()
    {
        nav.speed = walkSpeed;
        nav.ResetPath();
        isRunning = false;
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }
  

    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        nav.speed = walkSpeed;
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
        isChasing = false;
        isAttacking = false;
        isDead = true;
        nav.ResetPath();
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
