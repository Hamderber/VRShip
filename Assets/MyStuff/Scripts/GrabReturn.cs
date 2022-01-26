using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class GrabReturn : MonoBehaviour
{
    public Transform defaultTransform;
    public GameObject grabbableObject;
    public float speed = 1f;
    private void FixedUpdate()
    {
        if(!grabbableObject.GetComponent<XRGrabInteractable>().isSelected)
        {
            //Quaternion.identity.Set(defaultQuaternion.x, defaultQuaternion.y, defaultQuaternion.z, defaultQuaternion.w);
            //transform.SetPositionAndRotation(defaultTransform.position, defaultTransform.rotation);
            transform.position = Vector3.Lerp(transform.position, defaultTransform.position, speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultTransform.rotation, speed);
        }
    }
}
