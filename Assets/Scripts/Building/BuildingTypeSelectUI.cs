using UnityEngine.UI;
using UnityEngine;

public class BuildingTypeSelectUI : MonoBehaviour
{
    private void Awake()
    {
        var buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        Transform buildingTemplate = transform.Find("ButtonTemplate");
        buildingTemplate.gameObject.SetActive(false);

        int index = 0;
        const float defaultOffsetAmount = 20f;
        const float offsetAmount = 100f;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            Transform resourceTransform = Instantiate(buildingTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultOffsetAmount + (offsetAmount * index++), 10);
            resourceTransform.GetChild(1).GetComponent<Image>().sprite = buildingType.iconSprite;
        }
    }
}
