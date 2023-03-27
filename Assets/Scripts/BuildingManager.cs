using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{

    public static BuildingManager Instance;

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    //<>���� �κ����� Type�� ����
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
    //e �� EventArgs ������ �̺�Ʈ �߻��� ���õ� ������ ������ �ִ�.
    //�� �̺�Ʈ �ڵ鷯�� ����ϴ� �Ķ�����̴�.
    //���� �� ���콺 Ŭ�� �̺�Ʈ�ÿ� ���콺�� Ŭ���� ���� ��ǥ�� �˰� �ʹٴ���
    //������ ���� ��ư���� ������ ��ư������ �˰� ���� �� e�� ������ ���� �ϸ� �� ���̴�.

    //�̺�Ʈ ó����(Event Handler) �� �̺�Ʈ�� ���ε��Ǵ� �޼����̴�.
    //�̺�Ʈ�� �߻��ϸ� �̺�Ʈ�� ����� �̺�Ʈ ó������ �ڵ尡 ����ȴ�.
    //��� �̺�Ʈ ó����� ���� ���� �� ���� �Ű������� �����Ѵ�.
    //���콺 Ŭ���Ҷ� ����Ÿ���� � ������ �˷��ִ� �� �ƴ�?

    //���� ���·� Ŭ������ ����� ������ EventArgs�� ������ �κ��� e�� �ް� e�� ���� ������ �ڵ带 ������ �� �ִ�.



    [SerializeField] private Building hqBuilding;


    private Camera mainCamera; //ī�޶� ���� why ī�޶� �ϳ��� ���� ���� ���� �ִ�
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType; //BuildingTypeSO Ÿ���� buildingType�� ����

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        //�̸��� Ÿ�Ը�� ���ٸ� typeof�� �̿��Ͽ� �ҷ��� �� �ִ�. typeof(BuildingTypeListSO).Name�� BuildingTypeListSO�� �ҷ���
        //buildingTypeList�� ���� �־��� 
    }

    private void Start()
    {
        mainCamera = Camera.main; //Camera.main�� �ִ� ���� ȿ���� ����
        //UtilsClass�� �ű�鼭 �ʿ䰡 ������
        //Resources.Load<BuildingTypeListSO>("BuildlingTypsList");
        //BuidlingTypeListSO Ÿ���� BuildingTypeList��� �̸��� ���� ��ü �ҷ�����

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
        //EventSystem.current.IsPointerOverGameObject() = �����Ͱ� EventSystem�� ���� �ִ��� Ȯ��
        {
            //Instantiate(transform , vector3, quaternion)
            if (activeBuildingType != null)
            //activeBuildingType�� ���� �ƴϸ� ����
            {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string erroMessage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        //buildingType.prefab�� scriptableObject�� ������ BuildingTypeSo�� ������Ʈ �� ���õ� ������Ʈ�� prefab

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
        //activeBuildingType�� buildingType�� ����
        //arrowBtn�̸� null�� ����
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
        //null�� �ƴϸ� ��
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
        //activeBuildingType�� ��ȯ

    }


    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    //https://docs.microsoft.com/ko-kr/dotnet/csharp/language-reference/keywords/out-parameter-modifier
    //out �Ű������� �̰� ����ϸ� �Լ� ���� �������� ���� ���� �� ����

    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        //Box��� position�� �߽����� boxCollider2D�� size��ŭ ȸ���� 0
        //OverlapBoxAll(��ġ(Vector2), ������(Vector2), ����(float))
        //�������� collider2DArray�� �迭�� �־���

        bool isAreaClear = collider2DArray.Length == 0;
        //collider2DArray.Length�� 0�̶� ���Ͽ� 0�̶�� ���� ��ȯ �ƴϸ� ������ ��ȯ   
        if (!isAreaClear)
        {
            errorMessage = "Area is not Clear!";
            return false;
        }
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        //���� ��ġ�°� �迭�� �������

        foreach (Collider2D collider2D in collider2DArray)
        //collider2DArray��ŭ �ݺ�
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            //buildingTypeHolder�� null�� �ƴϸ� 
            {
                if (buildingTypeHolder.buildingType == buildingType)
                //buildingTypeHolder.buildingType�� buildikngType�� ������ false�� return�Ͽ� ��ġ �Ұ�
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
        //���� ��ġ�°� �迭�� �������

        foreach (Collider2D collider2D in collider2DArray)
        //collider2DArray��ŭ �ݺ�
        {
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            //buildingTypeHolder�� null�� �ƴϸ� 
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
