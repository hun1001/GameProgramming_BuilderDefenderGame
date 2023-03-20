using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    private ResourceGenerator resourceGenerator = null;

    [Header("Settings")]
    [SerializeField]
    private SpriteRenderer _icon = null;

    [SerializeField]
    private Transform _barTransform = null;

    [SerializeField]
    private TMPro.TMP_Text _textMesh = null;

    void Start()
    {
        resourceGenerator = GetComponentInParent<ResourceGenerator>();
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.ResourceGeneratorData;

        _icon.sprite = resourceGeneratorData.resourceType.iconSprite;
        _textMesh.text = resourceGenerator.AmountGeneratedPerSecond.ToString("F1");
    }

    void Update()
    {
        _barTransform.localScale = new Vector3(resourceGenerator.TimerNormalized * 1.5f, 1, 1);
    }
}
