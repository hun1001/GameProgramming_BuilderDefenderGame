using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipUICanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;
    // [SerializeField]
    // private RectTransform _rectTransform;

    [SerializeField]
    private float _deleteDelay = 0.5f;

    public ToolTipUICanvas SetText(string toolTipText)
    {
        _textMeshPro.SetText(toolTipText);
        _textMeshPro.ForceMeshUpdate();

        // 텍스트 길이에 맞춰 크기 변경해주는 코드
        // Vector2 textSize = _textMeshPro.GetRenderedValues(false);

        // Vector2 paddingSize = new Vector2(8, 8);

        // Vector2 backgroundSize = textSize + paddingSize;

        // _rectTransform.sizeDelta = backgroundSize;

        StartCoroutine(AutoDelete());
        return this;
    }

    private IEnumerator AutoDelete()
    {
        yield return new WaitForSeconds(_deleteDelay);
        Destroy(gameObject);
    }
}