using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{

    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;
    //Dictionary<T,Q> T�� Q�� �� �ڷ������� Dictionary�� ����

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //ResourceTypeeListSO�� ������ resourceTypeList��� ������ ����. resourceTypeList��� ������ Resources.Load�� ResourceTypeListSO�� ������ �ҷ���, typeof().Name �̸��� �ҷ���

        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();
        //resourceTypeTrannsformDictionary�� ����

        Transform resourceTemplate = transform.Find("resourceTemplate");
        //Transform ������ resourceTemplate�� transform ������ resourceTemplate��� �̸��� ã�ƿ�
        //[SerializeField] private Transform���ٴ� Ȯ���� Find�� �� ��ȣ������ �˾Ƽ� ���� 

        resourceTemplate.gameObject.SetActive(false);
        //resourceTemplate�� ����� ���ø��̴�. �̰��� �⺻ ���ø��� SetActive�� ����.
        //SetActive�� gameObject�� ���ؼ� ����� �� �ִ�. SetActive�� ������Ʈ�� ���� �� ���� ��Ÿ����.

        int index = 0;
        //�� �� °���� �� ����

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        //foreach (�ڷ��� ������ in ��ŭ ����(��))
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            //Instantiate�� ���� �Լ��̴�. Instantiate(������ Transform, ��ġ)
            resourceTransform.gameObject.SetActive(true);
            //������ resoureTransform�� SetActive�� Ų��.

            float offsetAmount = -160f;
            //offsetAmount�� ������ Transform�� ��ġ���� ���� �� ����ϰ� �� 
            //offsetAmount�� index�� ���� ��ŭ �������� ��

            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            //resourceTransform�� RectTransform�� anchoredPosition�� Vector���� ����

            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;
            //���� �� resourceTransform���� ���� ������Ʈ�� image��� �̸��� Image�� ������. Find�� �̿��Ͽ� image�� ã��
            //GetComponent�� ����Ͽ� Image�� sprite�� resourceType�� sprite�� ���� ����;

            resourceTypeTransformDictionary[resourceType] = resourceTransform;
            //resourceTypeTransformDictionary[ResourceTypeSO������ ��]�� ������ Transform�� ����ִ´�.
            //������� ���� �� Transform�� resourceTypeTransformDictionary�� �̿��Ͽ� �����Լ��� ���� �Լ�ó�� ���

            index++;
        }
    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        //�� ���� �� �ڵ� �ϼ� + �ڵ� �Լ� ����
        //OnResourceAmountChanged��� Event�� ResourceManager_OnResourceAmountChanged��� �Լ� �ֱ�
        //Ŭ����-�̸�.�̺�Ʈ-�̸� += new �̺�Ʈ-�ڵ鷯(����-�޼���-�̸�) // �̺�Ʈ ���� 

        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    //ResourceManager�� AddResource �Լ��� ����� ������ Event�� ���� �� �Լ��� ����
    //sender � ������Ʈ�� ��������, e�� EventHandler�� ����ϴ� �Ķ����
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransform = resourceTypeTransformDictionary[resourceType];
            //Transform ������ resourceTransform�� �����Ͽ� resourceTypeTransformDictionary�� resourcType�� Ű�� �´� ���� ����

            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            //resourceAmount�� ����� ���ÿ� ���ǳ�����. ResourceManager�� GetResourceAmount�� �����Ŵ.
            //resourceType���¸� ������ ������ foreach�� ������ �°� �����Ŵ

            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            //������ resourceTransform���� ���� ������Ʈ�� text��� �̸��� TextMeshPro�� ������. Find�� �̿��Ͽ� text�� ã��
            //Getcomponent�� ����Ͽ� TextMeshProUIUI�� Ȱ���Ͽ� �ҷ���.
            //TextMeshProUGUI�� ����Ϸ��� TMPro�� using������ �����ؾ���
            //SetText() ��ȣ ���� ���ڿ��̸� ���
            //resourceAmount.ToString()�� ���ڿ��� ��ȯ

        }
    }

}
