using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private IEnumerator Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;

        foreach (var c in colliders)
        {
            if (c.TryGetComponent(out ResourceNode resourceNode))
            {
                if (resourceNode.CompareResourceType(resourceGeneratorData.resourceType))
                {
                    nearbyResourceAmount++;
                }
                break;
            }
        }

        if (nearbyResourceAmount == 0)
        {
            yield break;
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);

        while (true)
        {
            yield return new WaitForSeconds(timerMax);
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }
}
