using UnityEngine;
using TMPro;

public class ResourceAmountUI : MonoBehaviour
{
    private ResourceTypeSO resourceType;
    public ResourceTypeSO ResourceType
    {
        set => resourceType ??= value;
    }

    [SerializeField]
    private TMP_Text textComponent;

    private void Start() => ResourceManager.Instance.OnResourceAmountChanged += (e, a) => textComponent.text = ResourceManager.Instance.ResourceAmountDictionary[resourceType].ToString();
}
