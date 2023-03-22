using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceAmount
{
    [SerializeField]
    private ResourceTypeSO _resourceType = null;
    public ResourceTypeSO ResourceType => _resourceType;

    [SerializeField]
    private int _amount;
    public int Amount => _amount;
}
