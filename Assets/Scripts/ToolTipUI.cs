using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;
    [SerializeField]
    private RectTransform _rectTransform;

    // canvas와 월드 맞추려면 canvas사이즈로 나누기 position을

    public void SetText(string toolTipText)
    {
        _textMeshPro.SetText(toolTipText);
        _textMeshPro.ForceMeshUpdate();

        Vector2 textSize = _textMeshPro.GetRenderedValues(false);

        Vector2 paddingSize = new Vector2(8, 8);

        Vector2 backgroundSize = textSize + paddingSize;

        _rectTransform.sizeDelta = backgroundSize;
    }


}