using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipStatusPanel : MonoBehaviour
{
    [SerializeField] private TextMeshPro panelText;
    public ShipControl physicsShipController;
    public GameObject physicsShip;
    private void Awake()
    {
        panelText.text = $"Loading...";
    }
    public void UpdateDisplay()
    {
        panelText.text =
            $"Pitch {Mathf.FloorToInt(physicsShipController.pitch)}�\n" +
            $"Yaw {Mathf.FloorToInt(physicsShipController.yaw)}�\n" +
            $"Roll {Mathf.FloorToInt(physicsShipController.roll)}�\n" +
            $"Forward {Mathf.FloorToInt(physicsShipController.speedX)}km/hr\n" +
            $"Left/Right {-Mathf.FloorToInt(physicsShipController.speedZ)}km/hr\n" +
            $"Up/Down {Mathf.FloorToInt(physicsShipController.speedY)}km/hr\n" +
            $"Coordinates:\n{physicsShip.transform.position}";
    }
}
