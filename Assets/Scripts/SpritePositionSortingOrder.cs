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
        //sortingOrder수가 클수록 앞으로 나옴;
        //-를 붙여 y의 값이 높으면 (더 뒤쪽에 위치하면) -로 수가 더 작다

        //precisionMultiplier라는 수를 곱함으로써 수의 차이가 커지며 정밀도가 증가

        if (runOnce)
        {
            Destroy(this);
            //Destroy는 제거임 this니까 이 스크립트를 제거함
        }

    }



}
