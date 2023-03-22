using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField]
    private Sprite _arrowSprite = null;
    private Transform _arrowTransform = null;

    private Dictionary<BuildingTypeSO, Image> _buttonSelectDictionary = null;

    [SerializeField]
    private List<BuildingTypeSO> _ignoreBuildingTypeList = null;

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

        EventTrigger e = _arrowTransform.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;

        entry.callback.AddListener((data) =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
            UpdateActiveBuildingTypeButton();
        });

        e.triggers.Add(entry);

        AddInfoShowTrigger(e, EventTriggerType.PointerEnter, _arrowTransform.GetChild(3).gameObject, true);
        AddInfoShowTrigger(e, EventTriggerType.PointerExit, _arrowTransform.GetChild(3).gameObject, false);


        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (_ignoreBuildingTypeList.Contains(buildingType))
                continue;

            Transform buttonTransform = Instantiate(buildingTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(defaultOffsetAmount + (offsetAmount * index++), 10);
            buttonTransform.GetChild(1).GetComponent<Image>().sprite = buildingType.iconSprite;

            //buttonTransform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"W: {buildingType.constructionResourceCostArray[0].Amount}" + $"

            e = buttonTransform.GetComponent<EventTrigger>();

            AddPointerClickTrigger(e, EventTriggerType.PointerClick, buildingType);

            AddInfoShowTrigger(e, EventTriggerType.PointerEnter, buttonTransform.GetChild(3).gameObject, true);

            AddInfoShowTrigger(e, EventTriggerType.PointerExit, buttonTransform.GetChild(3).gameObject, false);

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

    private void AddPointerClickTrigger(EventTrigger e, EventTriggerType type, BuildingTypeSO buildingType)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;

        entry.callback.AddListener((data) =>
        {
            BuildingManager.Instance.SetActiveBuildingType(buildingType);
            UpdateActiveBuildingTypeButton();
        });

        e.triggers.Add(entry);
    }

    private void AddInfoShowTrigger(EventTrigger e, EventTriggerType type, GameObject info, bool isShow = true)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;

        entry.callback.AddListener((data) =>
        {
            info.SetActive(isShow);
        });

        e.triggers.Add(entry);
    }
}
