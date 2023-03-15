using UnityEngine;
using UnityEngine.EventSystems;
using System;

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
            if (activeBuildingType != null && CanSpawnHarvester())
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

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool CanSpawnHarvester()
    {
        BoxCollider2D boxCollider2D = activeBuildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(GetMouseWorldPosition() + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0f);

        return colliders.Length == 0;
    }
}
