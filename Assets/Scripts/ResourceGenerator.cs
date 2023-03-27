using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDatectionRadius);
        //Collider2D������ �迭�� collider2DArray�� Physics2D.OverlapCircleAll���� ���� ���� ����
        //Physics2D.OverlapCircleAll(������Ʈ�� ��ġ(���� �߽�), ������) ������Ʈ�� ��ġ�� �߽����� ���� �����
        //�� ���� �浹�ϴ� ������Ʈ�� �迭�� ���·� ����

        int nearbyResourceAmount = 0;

        foreach (Collider2D collider2D in collider2DArray)
        //�迭�� ����ŭ foreach�� ����
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            //resourceNode�� Null�� �ƴϰ�
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
        //buildingType�� buildingTypeHolder ��ũ��Ʈ�� buildingType.
        timerMax = resourceGeneratorData.timerMax;
        //buildingType�� resourceGeneratorData�� timerMax���� timerMax�� �ִ´�.
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        if (nearbyResourceAmount == 0)
        {
            enabled = false;
            //enabled�� Component�� �Ѱ� ���� ��
            //SetActive�� GameObject�� �Ѱ� ���� ��
            //ó�� ��ġ���� �� �ֺ��� �ڿ��� ���ٸ� Component�� ��
        }
        else
        {
            timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime; //Ÿ�̸� �ڵ� 
        if (timer <= 0f)
        {
            //timer�� 0�� �ɽ� 
            timer += timerMax;
            //timer�� timerMax�� ����.
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
