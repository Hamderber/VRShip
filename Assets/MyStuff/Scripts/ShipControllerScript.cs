using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShipControllerScript : MonoBehaviour
{
    public Transform joystickValue;//omnidirectional joystic controller
    public float leverValue;//lever controller
    public float switchInUpPosition;//switch (up or down) 1f = up 0f = down
    public float buttonPressed;//button controller 1f = true 0f = false
    [Tooltip("(Case sensitive) Controller types: joystick, lever, switch, button")]
    public string controllerType;
    public List<string> controllerTypes = new() { "joystick", "lever", "switch", "button" };//enum
    private bool _validController = false;
    public GameObject grabbableGameObject;
    private bool _debugThisScript = true;
    private Debugger _console;
    private void Start()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Debugger>();
    }
    private void Awake()
    {
        DetermineControllerType();
    }

    private void DetermineControllerType()
    {
        if(controllerType == null)
        {
            _console.Log(_debugThisScript, startOfMessage: $"Controller type for the controller called {name} is unassigned");
            return;
        }
        if(!controllerTypes.Contains(controllerType))
        {
            _console.Log(_debugThisScript, startOfMessage: $"Controller type for the controller called {name} is an invalid option");
            return;
        }
        _validController = true;
    }

    public float GetControllerValue()
    {
        if (!_validController) return 0;//return 0f if invalid controller
        if(controllerType == "joystick")
        {
            return 0;//todo
        }
        return 0;
    }

    private float ClampRotation(float rotation)
    {
        return (float)Math.Round(rotation * 360, 2);

    }



    /// <summary>
    /// DOUBLE
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    public float GetJoystickValue(char axis, bool checkIfHeld = false)
    {
        if (!_validController) return 0f;//return 0f if invalid controller
        if (controllerType != "joystick")
        {
            return 0f;//todo
        }
        bool isHeld = (grabbableGameObject.GetComponent<XRGrabInteractable>() != null) ? grabbableGameObject.GetComponent<XRGrabInteractable>().isSelected : false;
        if (checkIfHeld && isHeld)
        {
            
            if (axis == 'x') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.x), -150, 150);
            if (axis == 'y') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.y), -150, 150);
            if (axis == 'z') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.z), -150, 150);
        }
        if(!checkIfHeld)
        {
            if (axis == 'x') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.x), -150, 150);
            if (axis == 'y') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.y), -150, 150);
            if (axis == 'z') return Math.Clamp(ClampRotation(joystickValue.transform.localRotation.z), -150, 150);
        }
        return 0f;
    }

}
