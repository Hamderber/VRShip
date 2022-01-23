using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControl : MonoBehaviour
{
    [SerializeField]private GameObject _lookTarget;
    private void Start()
    {
        _lookTarget = GameObject.Find("LocalCenter");
    }
    public void RefreshAnchoring()
    {
        transform.LookAt(_lookTarget.transform);
        transform.rotation *= Quaternion.Euler(0f, 90f, 90f);
    }
    void FixedUpdate()
    {
        RefreshAnchoring();
    }
}
