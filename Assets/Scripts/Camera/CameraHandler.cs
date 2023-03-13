using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;

    [Header("Values")]
    [SerializeField]
    private float _cameraSpeed = 10f;

    [SerializeField]
    private float _zoomAmount = 2f;

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * _cameraSpeed * Time.deltaTime;
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(_cinemachineVirtualCamera.m_Lens.OrthographicSize + (-Input.mouseScrollDelta.y * _zoomAmount * Time.deltaTime), 5, 30);
    }
}
