using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    public ResourceGeneratorData ResourceGeneratorData => resourceGeneratorData;

    private float timerMax;
    private float timer;
    public float TimerNormalized => timer / timerMax;

    public float AmountGeneratedPerSecond => 1f / timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private IEnumerator Start()
    {
        int nearbyResourceAmount = GetNearByResourceAmount();

        if (nearbyResourceAmount == 0)
        {
            yield break;
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        timerMax = (resourceGeneratorData.timerMax / 2f) + resourceGeneratorData.timerMax * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        timer = 0;

        while (true)
        {
            yield return null;
            timer += Time.deltaTime;

            if (timer >= timerMax)
            {
                timer = 0;
                ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
            }
        }
    }

    public int GetNearByResourceAmount()
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
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceAmount;
    }
}
