using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ShipCore : MonoBehaviour
{

    [SerializeField] private GameObject _placementPreview;

    public void TogglePlacementPreview()
    {
        if (gameObject.GetComponent<XRGrabInteractable>().isSelected)
        {
            _placementPreview.SetActive(true);
        }
        else
        {
            _placementPreview.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        TogglePlacementPreview();
    }
}
