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

    private ResourceGenerator _resourceGenerator = null;

    public void SetActive(ResourceGenerator resourceGenerator)
    {
        if (resourceGenerator is null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
            _resourceGenerator = resourceGenerator;
            _icon.sprite = resourceGenerator.ResourceGeneratorData.resourceType.iconSprite;
        }
    }

    private void Update()
    {
        int nearbyResourceAmount = _resourceGenerator.GetNearByResourceAmount();
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / _resourceGenerator.ResourceGeneratorData.maxResourceAmount * 100f);
        _textMesh.text = percent.ToString() + "%";
    }
}
