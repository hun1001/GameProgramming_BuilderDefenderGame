using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{

    public static BuildingManager Instance;

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    //<>안의 부분으로 Type을 지정
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
    //e 는 EventArgs 형으로 이벤트 발생과 관련된 정보를 가지고 있다.
    //즉 이벤트 핸들러가 사용하는 파라미터이다.
    //예를 들어서 마우스 클릭 이벤트시에 마우스가 클릭된 곳의 좌표를 알고 싶다던가
    //마우의 왼쪽 버튼인지 오른쪽 버튼인지를 알고 싶을 때 e의 내용을 참고 하면 될 것이다.

    //이벤트 처리기(Event Handler) 는 이벤트에 바인딩되는 메서드이다.
    //이벤트가 발생하면 이벤트와 연결된 이벤트 처리기의 코드가 샐행된다.
    //모든 이벤트 처리기는 위와 같은 두 개의 매개변수를 전달한다.
    //마우스 클릭할때 빌딩타입이 어떤 얘인지 알려주는 거 아님?

    //위의 형태로 클래스를 만들고 넣으면 EventArgs로 보내는 부분을 e로 받고 e를 통해 보내온 코드를 수정할 수 있다.



    [SerializeField] private Building hqBuilding;


    private Camera mainCamera; //카메라 선언 why 카메라를 하나만 쓰지 않을 수도 있다
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType; //BuildingTypeSO 타입을 buildingType로 선언

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        //이름이 타입명과 같다면 typeof를 이용하여 불러올 수 있다. typeof(BuildingTypeListSO).Name은 BuildingTypeListSO를 불러옴
        //buildingTypeList에 값을 넣어줌 
    }

    private void Start()
    {
        mainCamera = Camera.main; //Camera.main을 넣는 것이 효율이 좋음
        //UtilsClass로 옮기면서 필요가 없어짐
        //Resources.Load<BuildingTypeListSO>("BuildlingTypsList");
        //BuidlingTypeListSO 타입의 BuildingTypeList라는 이름을 가진 객체 불러오기

        hqBuilding.GetComponent<HealthSystem>().OnDied += BuildingManager_OnDied;
    }

    private void BuildingManager_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        //EventSystem.current.IsPointerOverGameObject() = 포인터가 EventSystem의 위에 있는지 확인
        {
            //Instantiate(transform , vector3, quaternion)
            if (activeBuildingType != null)
            //activeBuildingType이 널이 아니면 실행
            {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string erroMessage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        //buildingType.prefab은 scriptableObject로 생성한 BuildingTypeSo의 오브젝트 중 선택된 오브젝트의 prefab

                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCostString(), new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(erroMessage, new TooltipUI.TooltipTimer { timer = 2f });
                }
            }

        }
    }



    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        //activeBuildingType에 buildingType을 넣음
        //arrowBtn이면 null을 받음
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
        //null이 아니면 됨
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
        //activeBuildingType을 반환

    }


    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    //https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/out-parameter-modifier
    //out 매개변수자 이걸 사용하면 함수 밖의 변수들의 값을 정할 수 있음

    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        //Box모양 position을 중심으로 boxCollider2D의 size만큼 회전은 0
        //OverlapBoxAll(위치(Vector2), 사이즈(Vector2), 각도(float))
        //겹쳐지면 collider2DArray의 배열에 넣어짐

        bool isAreaClear = collider2DArray.Length == 0;
        //collider2DArray.Length와 0이랑 비교하여 0이라면 참을 반환 아니면 거짓을 반환   
        if (!isAreaClear)
        {
            errorMessage = "Area is not Clear!";
            return false;
        }
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        //원에 겹치는거 배열에 집어넣음

        foreach (Collider2D collider2D in collider2DArray)
        //collider2DArray만큼 반복
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            //buildingTypeHolder가 null이 아니면 
            {
                if (buildingTypeHolder.buildingType == buildingType)
                //buildingTypeHolder.buildingType과 buildikngType이 같으면 false를 return하여 설치 불가
                {
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }

        if (buildingType.hasResourceGeneratorData)
        {
            ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if(nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);
        //원에 겹치는거 배열에 집어넣음

        foreach (Collider2D collider2D in collider2DArray)
        //collider2DArray만큼 반복
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            //buildingTypeHolder가 null이 아니면 
            {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far from any oother buildings";
        return false;
    }

    public Building GetHQBuilding()
    {
        return hqBuilding;
    }
}
