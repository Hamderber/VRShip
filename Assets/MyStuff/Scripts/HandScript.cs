using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandScript : MonoBehaviour
{
    public string handName;// Name of hand
    public GameObject handInteractionReference;// GameObject reference of the physical hand the player sees
    public GameObject objectInHand = null;
    public string tagOfObjectInHand = null;
    public List<string> tagsOfHoldableObjects = new();
    public InputActionReference leftTriggerReference;
    public InputActionReference rightTriggerReference;

    private void Awake()
    {
        //if(handName.ToLower().Contains("left")
        //if(handName.ToLower().Contains("right")
        leftTriggerReference.action.started += TriggerButtonLeft;
        rightTriggerReference.action.started += TriggerButtonRight;
    }

    public void DebugHand()
    {
        if (handName != null && objectInHand != null && tagOfObjectInHand != null) Debug.Log($"Hand: {handName} holding {objectInHand.name} with a tag of {tagOfObjectInHand}");
        else Debug.Log($"Hand is empty");
    }

    /// <summary>
    /// <br>Sets <see cref="tagOfObjectInHand"/> to the <see cref="string"/> tag of the <see cref="GameObject"/> passed.</br>
    /// </summary>
    public void UpdateTagOfObjectInHand(GameObject ob)
    {
        if (ob == null) tagOfObjectInHand = null;
        else tagOfObjectInHand = ob.tag;
    }

    private bool CheckIfCorrectTag(string tag)
    {
        foreach (string tagToCheck in tagsOfHoldableObjects)
        {
            if (tagToCheck == tag) return true;
        }
        return false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(CheckIfCorrectTag(other.tag))
        {
            objectInHand = other.gameObject;
            UpdateTagOfObjectInHand(other.gameObject);
            DebugHand();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (objectInHand == null) return;
        if(other.gameObject == objectInHand)//theoretically prevents deleting the wrong object when hand interacts with multiple simultaneously
        {
            objectInHand = null;
            UpdateTagOfObjectInHand(null);
            DebugHand();
        }
    }

    private void TriggerButtonLeft(InputAction.CallbackContext context)
    {
        if (objectInHand == null) return;
        Debug.Log($"{name}'s held object is {objectInHand.name} with ID: {objectInHand.GetInstanceID()}");
        var scriptReference_ShipPart = objectInHand.GetComponent<ShipPart>();
        if(scriptReference_ShipPart != null) scriptReference_ShipPart.ChangeArmorMesh(-1);
    }

    private void TriggerButtonRight(InputAction.CallbackContext context)
    {
        if (objectInHand == null) return;
        Debug.Log($"{name}'s held object is {objectInHand.name} with ID: {objectInHand.GetInstanceID()}");
        var scriptReference_ShipPart = objectInHand.GetComponent<ShipPart>();
        if (scriptReference_ShipPart != null) scriptReference_ShipPart.ChangeArmorMesh(1);
    }
}
