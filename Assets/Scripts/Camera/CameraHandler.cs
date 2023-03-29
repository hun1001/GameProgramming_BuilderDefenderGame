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
        //���� ������ ���� ���� ū ����
        //�׷��� �Լ��� �����Ͽ� ���
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //GetAxis�� GetAxisRaw�� ����
        //GetAxis�� -1.0f���� 1.0f�̹Ƿ� �ε巯�� �̵��� ���
        //GetAxisRaw -1, 0, 1�̹Ƿ� Ű���带 ������ �� ��� �����ؾ� �� �� ���
        //Horizontal �� Vertical �� ����Ƽ���� ������ Ű������ Ű���� ������
        //���� ����

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
        //Vector2�� ���� �� x, y�� ��
        //Vector2 ������ = new Vector2(x��, y��)
        //.normalized�� ���͸� ����ȭ��Ŵ
        //����ȭ�� �밢�� �̵��� �Ұ��ϰ� �̵� �ӵ��� Ȯ��

        float moveSpeed = 30f;
        //�̵� �ӵ��� ���ϱ� ���Ͽ�

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        //transform.position�� ���´� Vector3�����̱� ������ Vector2�� moveDir�� 
        //Vector3���·� ��ȯ
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        //orthographicSize�� ���� mouseScrollDelta�� Y�� ���� ������
        //���콺 �ٿ� ���� ���� Ŀ���ų� �۾���
        //-�� ���� �ִ� ���� ���콺 ���� �ݴ�� �ۿ�Ǳ� ����
        //-�� ���Ƿ� ���� ���� �ۿ�

        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;
        //�Ǽ������� �ּڰ��� �ִ��� ������
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        //Mathf.Clamp�� �̿��Ͽ� �ּҰ��� �ִ밪 ���̿��� ���� �������� ��
        //Mathf[�����Լ��� �ǹ�].Clamp[�ּڰ��� �ִ� ���̷� ����](�˻��� ��, �ּڰ�, �ִ�)������ �Լ� ����

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
        //�ε巴�� ���ϱ� ���� targetOrthographicSize�� ����� orthographicSize�� �ϴ� �Ÿ� ����ϰ� 
        //Mathf.Lerp�� �̿��Ͽ� orthographicSize�� �ε巴�� ��
        //Mathf.Lerp(������ ��, ������, �ð�)

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        //m_Lens�� CineMachine�� Lens�̰� 
        //.OrthographicSize�� �Ÿ��̴�.
        //���� Ŭ���� ���̴� ������ �о�����
        //���� �������� ���̴� ������ �پ��
        //������ ���콺 �ٷ� ���� �����ִ� orthographicSize�� �Ÿ��� �����Ѵ�.
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
