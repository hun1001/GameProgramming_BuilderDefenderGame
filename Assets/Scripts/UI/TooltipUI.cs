using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform canvasRectTransform;

    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        Hide();
    }
    private void Update()
    {
        //마우스 위치에 툴팁이 뜸
        //마우스 위치가 툴팁 사이즈보다 크면 조정함
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;

        if(tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if(tooltipTimer.timer <= 0f)
            {
                Hide();
            }
        }
    }
    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);

        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize + padding;

    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
