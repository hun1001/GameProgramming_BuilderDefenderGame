using System.Collections.Generic;
using UnityEngine;

public class SortingSpritesManager : MonoSingleton<SortingSpritesManager>
{
    private List<SpriteRenderer[]> _spriteRenderers = null;

    protected override void Awake()
    {
        base.Awake();

        _spriteRenderers = new List<SpriteRenderer[]>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            _spriteRenderers.Add(transform.GetChild(i).GetComponentsInChildren<SpriteRenderer>());
        }
    }

    private void Start()
    {
        foreach (var s in _spriteRenderers)
        {
            int sortingOrder = -(int)(s[0].transform.position.y * 10);
            s[0].sortingOrder = sortingOrder;
            for (int i = 1; i < s.Length; ++i)
            {
                s[i].sortingOrder += sortingOrder;
            }
        }
    }
}
