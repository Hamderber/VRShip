using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandScript : MonoBehaviour
{

    private bool _debugThisScript = true;
    private Debugger _console;

    public string handName;// Name of hand
    public GameObject handInteractionReference;// GameObject reference of the physical hand the player sees
    public GameObject objectInHand = null;
    public string tagOfObjectInHand = null;
    public List<string> tagsOfHoldableObjects = new();
    public InputActionReference leftTriggerReference;
    public InputActionReference rightTriggerReference;

    private void Awake()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Debugger>();
        leftTriggerReference.action.started += TriggerButtonLeft;
        rightTriggerReference.action.started += TriggerButtonRight;
    }

    public void DebugHand()
    {
        if (!_debugThisScript) return;
        if (handName != null && objectInHand != null && tagOfObjectInHand != null) _console.Log(_debugThisScript, message: $"{handName} holding {objectInHand.name} with a tag of {tagOfObjectInHand}");
        else _console.Log(_debugThisScript, message: $"Hand is empty");
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
        if (other.name.Contains("Ship Core")) return;
        if(CheckIfCorrectTag(other.tag))
        {
            objectInHand = other.gameObject;
            UpdateTagOfObjectInHand(other.gameObject);
            DebugHand();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Ship Core")) return;
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
        _console.Log(_debugThisScript, message: $"{name}'s held object is {objectInHand.name} with ID: {objectInHand.GetInstanceID()}");
        var scriptReference_ShipPart = objectInHand.GetComponent<ShipPart>();
        if(scriptReference_ShipPart != null) scriptReference_ShipPart.ChangeArmorMesh(-1);
    }

    private void TriggerButtonRight(InputAction.CallbackContext context)
    {
        if (objectInHand == null) return;
        _console.Log(_debugThisScript, message: $"{name}'s held object is {objectInHand.name} with ID: {objectInHand.GetInstanceID()}");
        var scriptReference_ShipPart = objectInHand.GetComponent<ShipPart>();
        if (scriptReference_ShipPart != null) scriptReference_ShipPart.ChangeArmorMesh(1);
    }
}
