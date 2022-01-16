using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ShipPart : XRGrabInteractable
{
    private InputDevice targetDevice;

    void Start()
    {
        List<InputDevice> devices = new();
        InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Left;
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            Debug.Log($"{interactorsSelecting[0].transform.parent.name}");
        }

    }


}
