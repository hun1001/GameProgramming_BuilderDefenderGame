using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDatectionRadius);
        //Collider2D형태의 배열의 collider2DArray에 Physics2D.OverlapCircleAll에서 받은 값을 넣음
        //Physics2D.OverlapCircleAll(오브젝트의 위치(원의 중심), 반지름) 오브젝트의 위치를 중심으로 원을 만들고
        //그 원에 충돌하는 오브젝트를 배열의 형태로 보관

        int nearbyResourceAmount = 0;

        foreach (Collider2D collider2D in collider2DArray)
        //배열의 수만큼 foreach를 돌림
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            //resourceNode가 Null이 아니고
            {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }

    private ResourceGeneratorData resourceGeneratorData;


    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        //buildingType은 buildingTypeHolder 스크립트의 buildingType.
        timerMax = resourceGeneratorData.timerMax;
        //buildingType의 resourceGeneratorData의 timerMax값을 timerMax에 넣는다.
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        if (nearbyResourceAmount == 0)
        {
            enabled = false;
            //enabled은 Component를 켜고 끄는 것
            //SetActive는 GameObject를 켜고 끄는 것
            //처음 설치했을 때 주변에 자원이 없다면 Component를 끔
        }
        else
        {
            timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime; //타이머 코드 
        if (timer <= 0f)
        {
            //timer가 0이 될시 
            timer += timerMax;
            //timer에 timerMax를 더함.
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 100);

        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        return timer / timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / timerMax;
    }
}
