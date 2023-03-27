using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;
    private bool edgeScrolling;

    private void Awake()
    {
        Instance = this;

        edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }
    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        //보기 불편한 것이 가장 큰 문제
        //그래서 함수로 구분하여 사용
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //GetAxis와 GetAxisRaw의 차이
        //GetAxis는 -1.0f부터 1.0f이므로 부드러운 이동에 사용
        //GetAxisRaw -1, 0, 1이므로 키보드를 눌렀을 때 즉시 반응해야 할 때 사용
        //Horizontal 및 Vertical 은 유니티에서 정해준 키보드의 키들의 집합임
        //변경 가능

        float edgeScrollingSize = 30;
        if (edgeScrolling)
        {
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
            {
                x = 1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
            {
                y = 1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize)
            {
                y = -1f;
            }
        }


        //Vector2 moveDir = new Vector2(x, y).normalized;
        Vector3 moveDir = new Vector3(x, y).normalized;
        //Vector2는 벡터 값 x, y의 값
        //Vector2 변수명 = new Vector2(x값, y값)
        //.normalized는 벡터를 정규화시킴
        //정규화란 대각선 이동이 불가하고 이동 속도를 확인

        float moveSpeed = 30f;
        //이동 속도를 정하기 위하여

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        //transform.position의 형태는 Vector3형태이기 때문에 Vector2인 moveDir을 
        //Vector3형태로 변환
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        //orthographicSize의 값에 mouseScrollDelta의 Y의 값을 더해줌
        //마우스 휠에 따라 값이 커지거나 작아짐
        //-로 값을 넣는 이유 마우스 휠의 반대로 작용되기 때문
        //-를 쓰므로 휠의 정상 작용

        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;
        //실수형으로 최솟값과 최댓값을 정해줌
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        //Mathf.Clamp를 이용하여 최소값과 최대값 사이에서 값이 정해지게 함
        //Mathf[수학함수를 의미].Clamp[최솟값과 최댓값 사이로 지정](검사할 수, 최솟값, 최댓값)순으로 함수 정의

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        //부드럽게 줌하기 위해 targetOrthographicSize를 만들어 orthographicSize가 하던 거를 대신하고 
        //Mathf.Lerp를 이용하여 orthographicSize를 부드럽게 함
        //Mathf.Lerp(조정할 값, 조정값, 시간)

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        //m_Lens는 CineMachine의 Lens이고 
        //.OrthographicSize는 거리이다.
        //값이 클수록 보이는 면적이 넓어지고
        //값이 작을수록 보이는 면적이 줄어듦
        //위에서 마우스 휠로 값을 더해주는 orthographicSize를 거리에 대입한다.
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling()
    {
        return edgeScrolling;
    }
}
