using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _ghostSpriteRenderer = null;
    [SerializeField]
    private ResourceNearByOverlay _resourceNearByOverlay = null;

    private void Awake()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += (sender, e) =>
        {
            if (BuildingManager.Instance.ActiveBuildingType is null)
            {
                SetGhostSprite(null, false);
                _resourceNearByOverlay.SetActive(null);
            }
            else
            {
                SetGhostSprite(BuildingManager.Instance.ActiveBuildingType.iconSprite);
                _resourceNearByOverlay.SetActive(BuildingManager.Instance.ActiveBuildingType.resourceGeneratorData);
            }
        };
    }

    public void SetGhostSprite(Sprite sprite, bool isShow = true)
    {
        _ghostSpriteRenderer.sprite = sprite;
        _ghostSpriteRenderer.enabled = isShow;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
    }
}