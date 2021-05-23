using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatusController : MonoBehaviour
{
    //체력
    [SerializeField]
    private int hp;
    private int currentHP;

    //스테미나
    [SerializeField]
    private int sp;
    private int currentSP;

    //스테미나 증가량
    [SerializeField]
    private int spIncreaseSpeed;

    //스테미나 재회복 딜레이
    [SerializeField]
    private int spRechargeTime;
    private int currentSpRechargeTime;

    //스테미나 감소 여부
    private bool spUsed;

    //방어력
    [SerializeField]
    private int dp;
    private int currentDP;

    //배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    //배고픔이 줄어드는 속도
    [SerializeField]
    private int hungryDecreaseTime;
    private int currentHungryDecreaseTime;

    //목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    //목마름이 줄어드는 속도
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    //만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    //필요한 이미지
    [SerializeField]
    private Image[] imageGauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    public int CurrentSP => currentSP;

    private void Start()
    {
        currentHP = hp;
        currentDP = dp;
        currentSP = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    private void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GagueUpdate();
    }

    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
            {
                currentSpRechargeTime++;
            }
            else
                spUsed = false;
        }
    }
    
    private void SPRecover()
    {
        if (!spUsed && currentSP < sp)
        {
            currentSP += spIncreaseSpeed;
        }
    }

    private void Hungry()
    {
        if(currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("배고픔 수치가 0이 되었습니다.");
        }
    }

    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
        {
            Debug.Log("목마름 수치가 0이 되었습니다.");
        }
    }

    private void GagueUpdate()
    {
        imageGauge[HP].fillAmount = (float)currentHP / hp;
        imageGauge[DP].fillAmount = (float)currentDP / dp;
        imageGauge[SP].fillAmount = (float)currentSP / sp;
        imageGauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        imageGauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        imageGauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int count)
    {
        currentHP = Mathf.Min(currentHP + count, hp);
    }

    public void DecreaseHP(int count)
    {
        if(currentDP > 0)
        {
            DecreaseDP(count);
            return;
        }
        currentHP = Mathf.Max(currentHP - count, 0);
        if (currentHP == 0)
            Debug.Log("캐릭터의 hp가 0이 되었습니다!");
    }

    public void IncreaseDP(int count)
    {
        currentDP = Mathf.Min(currentDP + count, dp);
    }

    public void DecreaseDP(int count)
    {
        currentDP = Mathf.Max(currentDP - count, 0);
        if (currentDP == 0)
            Debug.Log("캐릭터의 방어력이 0이 되었습니다!");
    }

    public void IncreaseHungry(int count)
    {
        currentHungry = Mathf.Min(currentHungry + count, hungry);
    }

    public void DecreaseHungry(int count)
    {
        currentHungry = Mathf.Max(currentHungry - count, 0);
    }

    public void IncreaseThirsty(int count)
    {
        currentThirsty = Mathf.Min(currentThirsty + count, thirsty);
    }

    public void DecreaseThirsty(int count)
    {
        currentThirsty = Mathf.Max(currentThirsty - count, 0);
    }

    public void DecreaseStamina(int count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if(currentSP - count > 0)
        {
            currentSP -= count;
        }
        else
        {
            currentSP = 0;
        }
    }


}
