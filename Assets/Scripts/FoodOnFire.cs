using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOnFire : MonoBehaviour
{
    [SerializeField]
    private float time; //익히거나 타는데 걸리는 시간
    private float currentTime;

    private bool done; //끝났으면 더이상 불에 있어도 계산 무시 할 수있게

    [SerializeField]
    private GameObject goCookedItemPrefab; //완성된 혹은 탄 아이템 교체

    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fire" && !done)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= time)
            {
                done = true;
                Instantiate(goCookedItemPrefab, transform.position, Quaternion.Euler(transform.eulerAngles));
                Destroy(gameObject);
            }
        }
    }
}
