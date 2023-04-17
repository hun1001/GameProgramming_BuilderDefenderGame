using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;


    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    //Dictionary<1,2> �� 2�� ���� 1�� Ű�� �����. 1�� Ű�� ���� 2�� ���� ��µ�.

    private void Awake()
    {
        Instance = this;
        //�̱����� ����ϱ� ���� ���������� ������ ResourceManager�� Instance�� �� ������Ʈ�� ������.

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        //resourceAmountDictionary�� Dictionary<ResourceTypesSO, int> ������ Dictionary�� ����

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //Resources ������ ResourceTypeListSO��� �̸��� ResourceTypeListSO�� ����Ʈ�� �����´�

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
            //resourcTypeList�� ��ϸ�ŭ foreach�� ������ ResourceTypeSO Ÿ���� resourcType��
            //Dictionary�� ����Ͽ� Ű���� ��� 0���� �ʱ�ȭ.
        }

        foreach (ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }


    private void TestLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString + ": " + resourceAmountDictionary[resourceType]);
            //�̸��� ������ ���
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {

        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {

        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
}
