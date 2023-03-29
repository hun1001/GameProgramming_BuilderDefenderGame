using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{

    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
    //Dictionary<T,Q> T와 Q에 들어간 자료형들을 Dictionary로 선언

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //ResourceTypeeListSO의 형태의 resourceTypeList라는 변수를 선언. resourceTypeList라는 변수에 Resources.Load의 ResourceTypeListSO의 값들을 불러옴, typeof().Name 이름을 불러옴

        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();
        //resourceTypeTrannsformDictionary에 정의

        Transform resourceTemplate = transform.Find("resourceTemplate");
        //Transform 형태의 resourceTemplate에 transform 형태의 resourceTemplate라는 이름을 찾아옴
        //[SerializeField] private Transform보다는 확실한 Find를 더 선호하지만 알아서 선택 

        resourceTemplate.gameObject.SetActive(false);
        //resourceTemplate은 복사될 템플릿이다. 이것의 기본 템플릿의 SetActive을 끈다.
        //SetActive는 gameObject를 통해서 사용할 수 있다. SetActive는 오브젝트의 끄고 안 끄고를 나타낸다.

        int index = 0;
        //몇 번 째인지 쓸 변수

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        //foreach (자료형 변수명 in 얼만큼 할지(수))
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            //Instantiate는 복제 함수이다. Instantiate(복제할 Transform, 위치)
            resourceTransform.gameObject.SetActive(true);
            //복사한 resoureTransform의 SetActive를 킨다.

            float offsetAmount = -160f;
            //offsetAmount은 복제된 Transform의 위치값을 정할 때 사용하게 됨 
            //offsetAmount에 index를 곱한 만큼 왼쪽으로 감

            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            //resourceTransform에 RectTransform의 anchoredPosition의 Vector값을 설정

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            //복제 된 resourceTransform에는 하위 오브젝트로 image라는 이름의 Image가 존재함. Find를 이용하여 image를 찾아
            //GetComponent를 사용하여 Image의 sprite에 resourceType의 sprite를 집어 넣음;

            resourceTypeTransformDictionary[resourceType] = resourceTransform;
            //resourceTypeTransformDictionary[ResourceTypeSO형태의 값]에 복제한 Transform을 집어넣는다.
            //집어넣은 복제 된 Transform을 resourceTypeTransformDictionary를 이용하여 지역함수를 전역 함수처럼 사용

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        //탭 누를 시 자동 완성 + 자동 함수 생성
        //OnResourceAmountChanged라는 Event에 ResourceManager_OnResourceAmountChanged라는 함수 넣기
        //클래스-이름.이벤트-이름 += new 이벤트-핸들러(실행-메서드-이름) // 이벤트 연결 

        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    //ResourceManager의 AddResource 함수가 실행될 때마다 Event를 통해 이 함수가 실행
    //sender 어떤 오브젝트가 보내는지, e는 EventHandler가 사용하는 파라미터
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
            //Transform 형태의 resourceTransform을 선언하여 resourceTypeTransformDictionary의 resourcType의 키에 맞는 값을 정의

            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            //resourceAmount를 선언과 동시에 정의내리미. ResourceManager의 GetResourceAmount를 실행시킴.
            //resourceType형태를 보내기 때문에 foreach문 순서에 맞게 실행시킴

            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            //가져온 resourceTransform에는 하위 오브젝트로 text라는 이름의 TextMeshPro가 존재함. Find를 이용하여 text를 찾아
            //Getcomponent를 사용하여 TextMeshProUIUI를 활용하여 불러옴.
            //TextMeshProUGUI를 사용하려면 TMPro를 using문으로 정의해야함
            //SetText() 괄호 안이 문자열이면 출력
            //resourceAmount.ToString()을 문자열로 반환

        }
    }

}
