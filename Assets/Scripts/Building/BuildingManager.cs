using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    private static BuildingManager instance = null;
    public static BuildingManager Instance => instance;

    private BuildingTypeSO activeBuildingType;
    public BuildingTypeSO ActiveBuildingType => activeBuildingType;

    private BuildingTypeListSO buildingTypeList;

    private Camera mainCamera;

    private void Awake()
    {
        instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        activeBuildingType = buildingTypeList.list[0];
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Instantiate(activeBuildingType.prefab, GetMouseWorldPosition(), Quaternion.identity);
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

    public void SetActiveBuildingType(BuildingTypeSO buildingType) => activeBuildingType = buildingType;
}
