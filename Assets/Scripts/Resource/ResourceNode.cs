using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private ResourceTypeSO _resourceType = null;

    public bool CompareResourceType(ResourceTypeSO resourceType)
    {
        return _resourceType == resourceType;
    }
}
