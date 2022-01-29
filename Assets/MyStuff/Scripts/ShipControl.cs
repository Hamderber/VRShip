using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    [SerializeField] private List<string> _shipControlNameKeys = new();
    [SerializeField] private List<GameObject> _shipControlObjectValues = new();
    public Dictionary<string, GameObject> shipControlObjects = new();

    public float pitch = 0;
    public float roll = 0;
    public float speedX = 0;
    public float speedY = 0;
    public float speedZ = 0;
    public float yaw = 0;
    public ShipControllerScript shipControllerScriptRight;
    public ShipControllerScript shipControllerScriptLeft;

    public float maxRotation;//max degrees per second ship can rotate
    public float maxVelocity;
    public float velocityMultiplier;

    public float axialSensitivity = 15f;
    public float rotationalSensitivity;
    public float upDownMultiplier = 1.1f;
    public float rotationalMultiplier = 1f;

    public GameObject shipRoot;
    public Rigidbody shipRootRB;


    private bool _debugThisScript = true;
    private Debugger _console;

    private Transform _previousPositon = null;
    public Vector3 shipVelocity;
    public ShipStatusPanel shipStatusPanel;

    private void Start()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Debugger>();
        pitch = shipRoot.transform.rotation.x;
        yaw = shipRoot.transform.rotation.y;
        roll = shipRoot.transform.rotation.z;
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
        yaw = shipControllerScriptRight.GetJoystickValue('y', true);
        roll = shipControllerScriptRight.GetJoystickValue('z', true);
        pitch = shipControllerScriptRight.GetJoystickValue('x', true);
        speedX = shipControllerScriptLeft.GetJoystickValue('x');
        speedY = shipControllerScriptLeft.GetJoystickValue('y');
        speedZ = shipControllerScriptLeft.GetJoystickValue('z');
        //Debug.Log($"{speedX}, {speedY}, {speedZ}");
    }

    private float CalculateVelocity(float speed)
    {

        /*if (speed >= axialSensitivity)
        {
            speed = (speed >= axialSensitivity) ? speed : 0f;
            return velocityMultiplier * Mathf.Clamp(speed, -maxVelocity, maxVelocity);
        }
        if (speed <= -axialSensitivity)
        {
            speed = (speed <= -axialSensitivity) ? speed : 0f;
            return velocityMultiplier * Mathf.Clamp(speed, -maxVelocity, maxVelocity);
        }*/
        if (speed >= axialSensitivity) return speed = (speed > axialSensitivity) ? speed : 0f;
        if (speed <= -axialSensitivity) return speed = (speed < -axialSensitivity) ? speed : 0f;
        return 0f;
     }
    private float CalculateAngularVelocity(float rotation)
    {
        if (rotation >= rotationalSensitivity) rotation = (rotation > rotationalSensitivity) ? rotation : 0f;
        if (rotation <= -rotationalSensitivity) rotation = (rotation < -rotationalSensitivity) ? rotation : 0f;

        return rotation;
    }

    private void DetermineShipVelocity()
    {
        pitch = rotationalMultiplier * CalculateAngularVelocity(pitch);// + shipRoot.transform.rotation.x;
        roll = rotationalMultiplier * CalculateAngularVelocity(roll);// + shipRoot.transform.rotation.z;
        yaw = - rotationalMultiplier * CalculateAngularVelocity(yaw);// + shipRoot.transform.rotation.y;
        speedZ = - CalculateVelocity(speedZ);
        speedY = CalculateVelocity(speedY * upDownMultiplier);
        speedX = CalculateVelocity(speedX);
    }

    public void ApplyMovementToShip()
    {
        DetermineShipVelocity();
        //shipRootRB.velocity = 
        //shipRootRB.velocity = new Vector3(-speedZ, speedY, speedX);//z inverted to make controller make sense
        
        //shipRoot.transform.position = shipRoot.transform.position + (shipRoot.transform.forward * speedX * velocityMultiplier);
        //shipRoot.transform.position = shipRoot.transform.position + (shipRoot.transform.up * speedY * velocityMultiplier);
        //shipRoot.transform.position = shipRoot.transform.position + (shipRoot.transform.right * speedZ * velocityMultiplier);
        shipRoot.transform.Translate(new Vector3(
            0f,//(speedZ * velocityMultiplier),
            0f,//(speedY * velocityMultiplier),
            (speedX * velocityMultiplier)),
            Space.Self);


        //shipRootRB.angularVelocity = new Vector3(pitch, yaw, roll);
        //shipRoot.transform.Rotate(new Vector3(-roll, -pitch, -yaw), Space.World);
        //shipRoot.transform.rotation = Quaternion.Euler(
        //    Mathf.Lerp(shipRoot.transform.rotation.x, shipRoot.transform.rotation.x + pitch * rotationalMultiplier, .5f),
        //    Mathf.Lerp(shipRoot.transform.rotation.y, shipRoot.transform.rotation.y + yaw * rotationalMultiplier, .5f),
        //    Mathf.Lerp(shipRoot.transform.rotation.z, shipRoot.transform.rotation.z + roll * rotationalMultiplier, .5f));
        shipRoot.transform.Rotate(
             pitch * rotationalMultiplier,//shipRoot.transform.rotation.x +
             yaw * rotationalMultiplier,//shipRoot.transform.rotation.y +
             roll * rotationalMultiplier,//shipRoot.transform.rotation.z +
            Space.Self);

        //Quaternion.Euler(shipRoot.transform.rotation.x + pitch, shipRoot.transform.rotation.y + yaw, shipRoot.transform.rotation.z + roll);
        //shipRoot.transform.Rotate(shipRoot.transform.rotation.x / maxRotation, shipRoot.transform.rotation.y /maxRotation, shipRoot.transform.rotation.z /maxRotation);
        //shipRootRB.MovePosition(new Vector3(shipRoot.transform.position.x + (float)speedX / maxForce, shipRoot.transform.position.y /*+ (float)speedY / maxForce*/, shipRoot.transform.position.z /*+ (float)speedZ / maxForce*/));//shipRootRB.MovePosition(new Vector3((float)speedX / maxForce, (float)speedY / maxForce, (float)speedZ / maxForce));
    }

    private void FixedUpdate()
    {
        
        UpdateControlShipControlInput();
        ApplyMovementToShip();
        shipStatusPanel.UpdateDisplay();
        
    }
}
