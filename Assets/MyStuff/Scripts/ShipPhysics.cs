using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPhysics : MonoBehaviour
{
    public GameObject shipPhysicsObject;

    private void FixedUpdate()
    {
        transform.position = shipPhysicsObject.transform.position;
        transform.rotation = shipPhysicsObject.transform.rotation;
    }
}
