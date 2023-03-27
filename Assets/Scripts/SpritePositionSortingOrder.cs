using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce;
    [SerializeField] private float positionOffsetY;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void LateUpdate()
    {
        float precisionMultiplier = 5f;

        spriteRenderer.sortingOrder = (int)(-(transform.position.y + positionOffsetY) * precisionMultiplier);
        //sortingOrder���� Ŭ���� ������ ����;
        //-�� �ٿ� y�� ���� ������ (�� ���ʿ� ��ġ�ϸ�) -�� ���� �� �۴�

        //precisionMultiplier��� ���� �������ν� ���� ���̰� Ŀ���� ���е��� ����

        if (runOnce)
        {
            Destroy(this);
            //Destroy�� ������ this�ϱ� �� ��ũ��Ʈ�� ������
        }

    }



}
