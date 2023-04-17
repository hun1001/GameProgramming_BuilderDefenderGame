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
    //Dictionary<1,2> 는 2의 값을 1을 키로 사용함. 1의 키를 쓰면 2의 값이 출력됨.

    private void Awake()
    {
        Instance = this;
        //싱글톤을 사용하기 위해 정적변수로 선언한 ResourceManager의 Instance가 이 오브젝트임 정의함.

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();
        //resourceAmountDictionary를 Dictionary<ResourceTypesSO, int> 형태의 Dictionary를 정의

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //Resources 파일의 ResourceTypeListSO라는 이름을 ResourceTypeListSO의 리스트에 가져온다

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
            //resourcTypeList의 목록만큼 foreach를 돌리며 ResourceTypeSO 타입의 resourcType를
            //Dictionary를 사용하여 키값을 모두 0으로 초기화.
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
            //이름과 갯수를 출력
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
