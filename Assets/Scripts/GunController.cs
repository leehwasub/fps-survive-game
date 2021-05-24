using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static bool isActivate = false;

    // 현재 장착도니 총
    [SerializeField]
    private Gun currentGun;

    // 연사 속도 계산
    private float currentFireRate;

    //상태 변수
    private bool isReload = false;
    private bool isFineSightMode = false;

    // 본래 포지션 값
    private Vector3 originPos;

    //효과음 재생
    private AudioSource audioSource;

    //충돌 정보 받아옴
    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    //필요한 컴포넌트
    [SerializeField]
    private Camera myCamera;
    private CrossHair crossHair;

    //피격 이펙트
    [SerializeField]
    private GameObject hitEffectPrefab;

    public Gun Gun => currentGun;
    public bool IsFineSightMode => isFineSightMode;


    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        crossHair = FindObjectOfType<CrossHair>();

    }

    private void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            //TryFire();
            TryReload();
            TryFindSight();
        }
    }

    // 연사속도 계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1프레임에 몇초인지
        }
    }

    // 발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    // 발사전 계산
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0) Shoot();
            else
            {
                CancelFindSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Shoot()
    {
        crossHair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 연사 속도 재계산
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();
        //총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }



    private void Hit()
    {
        if(Physics.Raycast(myCamera.transform.position, myCamera.transform.forward +
            new Vector3(UnityEngine.Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy)
            , UnityEngine.Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy), 0), out hitInfo, currentGun.range
            , layerMask))
        {
            GameObject clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    //재장전 시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFindSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 재장전
    private IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총일이 없습니다.");
        }
    }

    // 정조준 시도
    private void TryFindSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FireSight();
        }
    }

    //정조준 취소
    public void CancelFindSight()
    {
        if (isFineSightMode) FireSight();
    }

    //정조준 로직 가동
    private void FireSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        crossHair.FineSightAnimation(isFineSightMode);
        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    // 정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    // 정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // 반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원 위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원 위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    // 사운드 재생
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void GunChange(Gun gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentGun = gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }

}
