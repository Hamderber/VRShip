using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeVelocity : MonoBehaviour
{
    public Rigidbody referenceObject;
    public Rigidbody localObject;

    private void FixedUpdate()
    {
        localObject.velocity -= referenceObject.velocity;
    }
}
