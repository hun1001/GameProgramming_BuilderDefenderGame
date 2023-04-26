using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField]
    private GameObject _default = null;

    [SerializeField]
    private GameObject _zoom = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _default.SetActive(!_default.activeSelf);
            _zoom.SetActive(!_zoom.activeSelf);
        }
    }
}
