using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _ghostSpriteRenderer = null;

    private void Awake()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += (sender, e) =>
        {
            if (BuildingManager.Instance.ActiveBuildingType is null)
            {
                SetGhostSprite(null, false);
            }
            else
            {
                SetGhostSprite(BuildingManager.Instance.ActiveBuildingType.iconSprite);
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
