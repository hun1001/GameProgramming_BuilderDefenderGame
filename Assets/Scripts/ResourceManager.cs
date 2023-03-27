using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    //싱글톤 get으로 값을 가져갈수 있지만, private이기 때문에 값을 설정할 수 없음.

    //위 코드의 원래 코드
    /*
    private static ResourceManager instance;
    public static ResourceManager GetInstance() {
        return instance;
    }
    private static void SetInstance(ResourceManager set) {
        instance = set;
    }
    */

    public event EventHandler OnResourceAmountChanged;
    //event를 사용하려면 using Systema를 선언
    //public event EventHandler 이벤트-이름 // 이벤트 정의


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
        //아래 코드와 같은 의미
        //OnResourceAmountChanged라는 Event에 어떤 함수도 들어오지 않으면 
        /*if(OnResourceAmountChanged != null)
        {
            OnResourceAmountChanged(this, EventArgs.Empty);
        }*/
        //this는 이 오브젝트가 보낸다는 뜻, EventArgs.Empty는 Event파라미터로 보낼 것이 없을 때
        //null로 보낼 시 e가 받았고 e로 무언가를 시도할 때 발생하는 잠재적인 Null 참조 오류를 피하기 위함.

        //resourceAmountDictionary[resourceType]의 키값에 amount만큼 더한다.
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    //ResourceTypeSO형태로 매개변수를 받아 resourceAmountDictionary에 사용
    {
        return resourceAmountDictionary[resourceType];
        //int형으로 함수를 선언했기 때문에 반환을 해야한다.
        //resourceType의 키 값을 resourceAmountDictionary를 return으로 반환한다.
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
