using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipStatusPanel : MonoBehaviour
{
    [SerializeField] private TextMeshPro panelText;
    public double pitch = 0;
    public double roll = 0;
    public double speedX = 0;
    public double speedY = 0;
    public double speedZ = 0;
    public double yaw = 0;
    public ShipControllerScript shipControllerScriptRight;
    public ShipControllerScript shipControllerScriptLeft;
    public ShipControl physicsShipController;
    public GameObject physicsShip;

    void UpdateDisplay()
    {
        panelText.text =
            $"Pitch {Mathf.FloorToInt(physicsShipController.pitch)}º\n" +
            $"Yaw {Mathf.FloorToInt(physicsShipController.yaw)}º\n" +
            $"Roll {Mathf.FloorToInt(physicsShipController.roll)}º\n" +
            $"Forward {Mathf.FloorToInt(physicsShipController.speedX)}km/hr\n" +
            $"Left/Right {-Mathf.FloorToInt(physicsShipController.speedZ)}km/hr\n" +
            $"Up/Down {Mathf.FloorToInt(physicsShipController.speedY)}km/hr\n" +
            $"Coordinates:\n{physicsShip.transform.position}";
    }

    private void UpdateControlShipControlInput()
    {
        yaw = shipControllerScriptRight.GetJoystickValue('y');
        roll = shipControllerScriptRight.GetJoystickValue('z');
        pitch = shipControllerScriptRight.GetJoystickValue('x');
        speedX = shipControllerScriptLeft.GetJoystickValue('x');
        speedY = shipControllerScriptLeft.GetJoystickValue('y');
        speedZ = shipControllerScriptLeft.GetJoystickValue('z');
    }

    private void FixedUpdate()
    {
        //UpdateControlShipControlInput();
        UpdateDisplay();
    }
}
