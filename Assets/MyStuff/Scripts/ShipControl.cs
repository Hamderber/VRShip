using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField] private List<string> _shipControlNameKeys = new();
    [SerializeField] private List<GameObject> _shipControlObjectValues = new();
    public Dictionary<string, GameObject> shipControlObjects = new();

    public double pitch = 0;
    public double roll = 0;
    public double speedX = 0;
    public double speedY = 0;
    public double speedZ = 0;
    public double yaw = 0;
    public ShipControllerScript shipControllerScriptRight;
    public ShipControllerScript shipControllerScriptLeft;

    public float maxRotation = 15f;//max degrees per second ship can rotate
    public float maxForce = 15f;

    public GameObject shipRoot;
    public Rigidbody shipRootRB;


    private bool _debugThisScript = true;
    private Debugger _console;

    private void Start()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Debugger>();
    }

    private void BuildControlDictionary()
    {
        if(_shipControlNameKeys.Count != _shipControlObjectValues.Count)
        {
            _console.Log(_debugThisScript, startOfMessage: $"Ship control names and object references aren't the same length");
        }
        for(int x = 0; x < _shipControlNameKeys.Count; x++)
        {
            shipControlObjects.Add(_shipControlNameKeys[x], _shipControlObjectValues[x]);
        }
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

    public void ApplyRotationToShip()
    {
        shipRoot.transform.Rotate(shipRoot.transform.rotation.x / maxRotation, shipRoot.transform.rotation.y /maxRotation, shipRoot.transform.rotation.z /maxRotation);
        shipRootRB.AddForce(new Vector3((float)speedX / maxForce, (float)speedY / maxForce, (float)speedZ / maxForce), 0);
    }

    private void FixedUpdate()
    {
        UpdateControlShipControlInput();
        ApplyRotationToShip();
    }
}
