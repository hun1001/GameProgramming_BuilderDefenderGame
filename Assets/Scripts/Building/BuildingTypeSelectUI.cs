using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{

    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;

    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;

    private Transform arrowBtn;

    private void Awake()
    {
        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;



        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = 130f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        //image라는 Transform을 찾고 Image 컴포넌트를 찾고 sprite를 buildingType의 sprite로 변경

        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
            //BuildingManager의 SetActiveBuildingType 함수를 실행하며 매개변수로 buildingType을 보냄
        });

        MouseEnterExitEvents mouseEnterExitEvents = arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("Arrow");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };

        index++;
        //선택되지 않은 것을 만들기 위하여 arrow를 만듬



        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;
            //ignoreBuildingTypeList에 속해있는 buildingType과 같다면 continue로 foreach넘기기
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = 130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;
            //image라는 Transform을 찾고 Image 컴포넌트를 찾고 sprite를 buildingType의 sprite로 변경

            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
                //BuildingManager의 SetActiveBuildingType 함수를 실행하며 매개변수로 buildingType을 보냄
            });

            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {

        arrowBtn.Find("selected").gameObject.SetActive(false);

        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        //btnTransformDictionary의 Keys만큼 반복됨
        {
            Transform btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
            //모든 selected를 SetActive로 끔
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        //지금 선택된 activeBuildingType을 가져와
        if (activeBuildingType == null)
        {
            arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
            //SetActive를 킴
        }
    }

}
