using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    private void Awake()
    {
        var resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        Transform resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 0;
        const float defaultOffsetAmount = -100f;
        const float offsetAmount = -160f;

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultOffsetAmount + (offsetAmount * index++), 0);
            resourceTransform.GetComponent<Image>().sprite = resourceType.iconSprite;
            resourceTransform.GetComponentInChildren<ResourceAmountUI>().ResourceType = resourceType;
        }
    }
}
