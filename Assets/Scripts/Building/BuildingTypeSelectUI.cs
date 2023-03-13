using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField]
    private Sprite _arrowSprite = null;
    private Transform _arrowTransform = null;

    private Dictionary<BuildingTypeSO, Image> _buttonSelectDictionary;

    private void Awake()
    {
        _buttonSelectDictionary = new Dictionary<BuildingTypeSO, Image>();

        var buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        Transform buildingTemplate = transform.Find("ButtonTemplate");
        buildingTemplate.gameObject.SetActive(false);

        int index = 0;
        const float defaultOffsetAmount = 20f;
        const float offsetAmount = 140f;

        _arrowTransform = Instantiate(buildingTemplate, transform);
        _arrowTransform.gameObject.SetActive(true);

        _arrowTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultOffsetAmount + (offsetAmount * index++), 10);
        _arrowTransform.GetChild(1).GetComponent<Image>().sprite = _arrowSprite;
        (_arrowTransform.GetChild(1).transform as RectTransform).sizeDelta = new Vector2(-20, -20);

        _arrowTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
            UpdateActiveBuildingTypeButton();
        });


        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            Transform buttonTransform = Instantiate(buildingTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultOffsetAmount + (offsetAmount * index++), 10);
            buttonTransform.GetChild(1).GetComponent<Image>().sprite = buildingType.iconSprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
                UpdateActiveBuildingTypeButton();
            });

            _buttonSelectDictionary[buildingType] = buttonTransform.GetChild(2).GetComponent<Image>();
        }

        Invoke(nameof(UpdateActiveBuildingTypeButton), Time.deltaTime);
    }

    private void UpdateActiveBuildingTypeButton()
    {
        _arrowTransform.GetChild(2).gameObject.SetActive(BuildingManager.Instance.ActiveBuildingType == null);

        foreach (BuildingTypeSO buildingType in _buttonSelectDictionary.Keys)
            _buttonSelectDictionary[buildingType].enabled = (buildingType == BuildingManager.Instance.ActiveBuildingType);
    }
}
