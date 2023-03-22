using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNearByOverlay : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private SpriteRenderer _icon = null;

    [SerializeField]
    private TMPro.TMP_Text _textMesh = null;

    private ResourceGeneratorData _resourceGenerator = null;

    public void SetActive(ResourceGeneratorData resourceGenerator)
    {
        _resourceGenerator = resourceGenerator;
        if (resourceGenerator is null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            _icon.sprite = resourceGenerator.resourceType.iconSprite;
        }
    }

    private void Update()
    {
        if (_resourceGenerator is null)
        {
            return;
        }
        int nearbyResourceAmount = ResourceGenerator.GetNearByResourceAmount(transform, _resourceGenerator);
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / _resourceGenerator.maxResourceAmount * 100f);
        _textMesh.text = percent.ToString() + "%";
    }
}
