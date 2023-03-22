using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager instance = null;
    public static BuildingManager Instance => instance;

    private BuildingTypeSO activeBuildingType;
    public BuildingTypeSO ActiveBuildingType => activeBuildingType;

    public event EventHandler OnActiveBuildingTypeChanged;

    private BuildingTypeListSO buildingTypeList;

    private Camera mainCamera;

    private void Awake()
    {
        instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        SetActiveBuildingType(null);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType is not null)
            {
                if (!CanSpawnBuilding(activeBuildingType, Input.mousePosition, out string a))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    ToolTipUICanvas printToolTip = Instantiate(Resources.Load<GameObject>("ToolTipUICanvas"), mousePosition, Quaternion.identity).transform.GetChild(0).GetComponent<ToolTipUICanvas>().SetText(a);
                }
                else if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    ToolTipUICanvas printToolTip = Instantiate(Resources.Load<GameObject>("ToolTipUICanvas"), mousePosition, Quaternion.identity).transform.GetChild(0).GetComponent<ToolTipUICanvas>().SetText("자원이 부족합니다.");
                }
                else
                {
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                    Instantiate(activeBuildingType.prefab, GetMouseWorldPosition(), Quaternion.identity);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeBuildingType = buildingTypeList.list[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeBuildingType = buildingTypeList.list[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeBuildingType = buildingTypeList.list[2];
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;

        if (!isAreaClear)
        {
            errorMessage = "건물을 놓을 수 없는 곳입니다.";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHoler = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHoler != null)
            {
                if (buildingTypeHoler.buildingType == buildingType)
                {
                    errorMessage = "같은 유형의 건물이 근처에 있습니다.";
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            BuildingTypeHolder buildingTypeHoler = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHoler != null)
            {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "다른 건물이 주변에 있어야 합니다.";
        return false;
    }
}
