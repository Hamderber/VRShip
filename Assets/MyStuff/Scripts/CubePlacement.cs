using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class CubePlacement : MonoBehaviour
{
    private Hashtable _shipPartIndex = new();
    private List<Vector3> _shipParts = new();
    [SerializeField]
    private GameObject[] _shipPartsVR = new GameObject[0];
    [SerializeField]
    private GameObject[] _shipPartsPlaced = new GameObject[0];
    private string[] _shipPartsPlacedString = new string[0];
    [SerializeField]
    private Vector3 blockRespawnPoint;
    [SerializeField]
    private string _buildTag;
    [SerializeField]
    private GameObject _shipCore;
    [SerializeField]
    private GameObject _previewPlacementObject;

    private Vector3 _localScale = Vector3.one;

    private bool _inPlacementField = false;
    private bool _placementPreviewEnabled = false;
    private GameObject _previewObject;
    private GameObject _interactableInPlacementField;

    public void Start()
    {
        for(int x = 0; x < _shipPartsVR.Length; x++)
        {
            //Debug.Log("Adding stuff to hash");
            _shipPartIndex.Add(_shipPartsVR[x].name, _shipPartsPlaced[x].name);
        }

        _shipPartsPlacedString = new string[_shipPartsVR.Length];
        for(int x = 0;x < _shipPartsVR.Length; x++)
        {
            _shipPartsPlacedString[x] = _shipPartsPlaced[x].name;
        }

    }
    private string CleanupName(string name)
    {
        if (name.IndexOf("(") == -1) return name;
        else if (name.IndexOf("(Clone)") != -1) return name.Substring(0, name.IndexOf("(Clone)"));
        else if (name.IndexOf("(Clone)") != -1) return name.Substring(0, name.IndexOf("(Clone)"));
        else return name[(name.IndexOf("("))..];
    }

    private Quaternion ClampAxisRotation(GameObject ob)
    {
        
        //DebugRotation(ob, "before");
        float x = ob.transform.localEulerAngles.x;
        float y = ob.transform.localEulerAngles.y;
        float z = ob.transform.localEulerAngles.z;
        //this is bad make this better later
        while (x < -360f || x > 360f)
        {
            if (x < -360f) x += 360f;
            if (x > 360f) x -= 360f;
        }

        if (x <= 45f)
        {
            x = 0f;
        }
        else if (x <= 135f)
        {
            x = 90f;
        }
        else if (x <= 225f)
        {
            x = 180f;
        }
        else if (x <= 315f)
        {
            x = 270f;
        }
        else x = 360f;

        

        //this is bad make this better later
        while (y < -360f || y > 360f)
        {
            if (y < -360f) y += 360f;
            if (y > 360f) y -= 360f;
        }
        if (y <= 45f)
        {
            y = 0f;
        }
        else if (y <= 135f)
        {
            y = 90f;
        }
        else if (y <= 225f)
        {
            y = 180f;
        }
        else if (y <= 315f)
        {
            y = 270f;
        }
        else y = 360f;

        
        //this is bad make this better later
        while (z < -360f || z > 360f)
        {
            if (z < -360f) z += 360f;
            if (z > 360f) z -= 360f;
        }
        if (z <= 45f)
        {
            z = 0f;
        }
        else if (z <= 135f)
        {
            z = 90f;
        }
        else if (z <= 225f)
        {
            z = 180f;
        }
        else if (z <= 315f)
        {
            z = 270f;
        }
        else z = 360f;

        //DebugRotation(ob, "after");
        return Quaternion.Euler(x, y, z);
    }

    private Vector3 ClampPosition(GameObject ob)
    {
        //DebugPosition(ob, "before");
        float x = ob.transform.localPosition.x;
        float y = ob.transform.localPosition.y;
        float z = ob.transform.localPosition.z;
        
        x = (float)Math.Round(x);
        y = (float)Math.Round(y);
        z = (float)Math.Round(z);
        //DebugPosition(ob, "after");
        return new Vector3(x,y,z);
    }
    private void DebugRotation(GameObject ob, string endStr = "")
    {
        Debug.Log($"Local rotation of {ob.name} x:{ob.transform.localEulerAngles.x} y:{ob.transform.localEulerAngles.y} z:{ob.transform.localEulerAngles.z} {endStr}");
        Debug.Log($"Global rotation of {ob.name} x:{ob.transform.eulerAngles.x} y:{ob.transform.eulerAngles.y} z:{ob.transform.eulerAngles.z} {endStr}");
    }
    private void DebugPosition(GameObject ob, string endStr = "")
    {
        Debug.Log($"Local position of {ob.name} x:{ob.transform.localPosition.x} y:{ob.transform.localPosition.y} z:{ob.transform.localPosition.z} {endStr}");
        Debug.Log($"Global rotation of {ob.name} x:{ob.transform.position.x} y:{ob.transform.position.y} z:{ob.transform.position.z} {endStr}");
    }
    private void ShowProjectedPlacement()
    {
        
        _placementPreviewEnabled = true;
        _previewObject = Instantiate(_previewPlacementObject, _interactableInPlacementField.transform.position, _interactableInPlacementField.transform.rotation);
        _previewObject.GetComponent<MeshFilter>().sharedMesh = _interactableInPlacementField.GetComponent<MeshFilter>().sharedMesh;
        
        //_previewObject.transform.localScale = (_localScale / 2);
        
        //_previewObject.transform.localScale = Vector3.one;
        _interactableInPlacementField.transform.localScale *= 0.5f;
        _previewObject.transform.parent = _interactableInPlacementField.transform;
        ClampObject(_previewObject);
    }
    private void ClampObject(GameObject ob)
    {

        ob.transform.parent = gameObject.transform;

        ob.transform.localRotation = ClampAxisRotation(ob);
        //DebugRotation(_shipCore);
        //DebugRotation(ob, "after clamp");

        ob.transform.localPosition = ClampPosition(ob);
        //DebugPosition(ob, "after clamp");
    }
    private void OnTriggerEnter(Collider other)
    {
        _inPlacementField = true;
        _interactableInPlacementField = other.gameObject;
        _interactableInPlacementField.name = CleanupName(_interactableInPlacementField.name);
        //Checks if the colliding object has the build tag that was defined in the editor,
        //Checks if the colliding object is contained in the hashtable by comparing the name of the object (removing the " (" and onwards from
        //the name as a way to check if it is a prefab
        
    }

    private void OnTriggerExit(Collider other)
    {
        _inPlacementField = false;
    }

    private void Update()
    {
        if (_previewObject != null)
        {
            _interactableInPlacementField.transform.localScale = _localScale;
            Destroy(_previewObject);
        }

        
        if (_interactableInPlacementField != null && _interactableInPlacementField.tag != null && _interactableInPlacementField.GetComponent<XRGrabInteractable>() != null)
        {

            if (_shipParts.Contains(_interactableInPlacementField.transform.localPosition)) return;

            if (_inPlacementField && _interactableInPlacementField.GetComponent<XRGrabInteractable>().isSelected && _interactableInPlacementField.tag == _buildTag && _shipPartIndex.ContainsKey(_interactableInPlacementField.name))
            {
                _localScale = _interactableInPlacementField.transform.localScale;
                ShowProjectedPlacement();
            }
            else if (_inPlacementField && !_interactableInPlacementField.GetComponent<XRGrabInteractable>().isSelected)
            {
                //Debug.Log("something in placement field and that thing -- IS NOT -- being held");
                if (_interactableInPlacementField.tag == _buildTag && _shipPartIndex.ContainsKey(_interactableInPlacementField.name))
                {
                    GameObject placedObject = Instantiate(_shipPartsPlaced[Array.IndexOf(_shipPartsPlacedString, _shipPartIndex[_interactableInPlacementField.name])], _interactableInPlacementField.transform.position, _interactableInPlacementField.transform.rotation);
                    ClampObject(placedObject);

                    GameObject respawnedObject = Instantiate(_interactableInPlacementField.gameObject, blockRespawnPoint, _interactableInPlacementField.gameObject.transform.rotation);

                    _shipParts.Add(placedObject.transform.localPosition);

                    Destroy(_interactableInPlacementField.gameObject);
                    _interactableInPlacementField = null;
                    Destroy(_previewObject);
                    _previewObject = null;
                }
            }
        }
        
    }
}

/*
 * Major issues:
 * 
 * Can have multiple objects placed in same spot potentially !!!! potentially fixed by using a list of occupied vector3's
 * Janky interaction of placement blocks when forced into cube (maybe a vr issue itself?)
 * 
 * 
 * 
 * 
 * 
 * 
 */