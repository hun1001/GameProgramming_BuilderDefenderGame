using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite iconSprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;

    public ResourceGeneratorData resourceGeneratorData;
}
