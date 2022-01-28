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

    void UpdateDisplay()
    {
        panelText.text =
            $"Pitch: (x){pitch}º\n" +
            $"Yaw: (y){yaw}º\n" +
            $"Roll: (z){roll}º\n" +
            $"Speed: (x) {speedX} km/hr\n" +
            $"Speed: (y) {speedY} km/hr\n" +
            $"Speed: (z) {speedZ} km/hr";
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
        UpdateControlShipControlInput();
        UpdateDisplay();
    }
}
