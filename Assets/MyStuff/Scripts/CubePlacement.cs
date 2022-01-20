using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class CubePlacement : MonoBehaviour
{
    [SerializeField] private List<Vector3> _shipParts = new();
    [SerializeField] private GameObject[] _shipPartsVR = new GameObject[0];
    [SerializeField] private GameObject[] _shipPartsPlaced = new GameObject[0];
    [SerializeField] private string[] _shipPartsPlacedNames = new string[0];
    [SerializeField] private Vector3 blockRespawnPoint;// World coordinate where new instances of placed blocks will spawn
    [SerializeField] private string _buildTag;// The tag of objects that can be attached to the ship
    [SerializeField] private GameObject _shipCore;// GameObject reference to the ship core (parent of the whole PHYSICAL ship)
    [SerializeField] private GameObject _previewPlacementObject;// GameObject reference to the preview placement object (NOT the preview object itself)
    [SerializeField] private Material _badPlacementMaterial;// Material reference for what a bad preview placement will be shown as
    private Hashtable _shipPartsHashtable = new();
    private List<Vector3> _shipPartsToAdd = new();
    private bool _placedInThisUpdate = false;
    private Vector3 _localScale = Vector3.one;
    private bool _inPlacementField = false;
    private GameObject _previewObject;
    private GameObject _interactableInPlacementField;

    /// <summary>
    /// <br>When script is loaded, adds all ship parts (<see cref="_shipPartsVR"/>) to hashtable (<see cref="_shipPartsHashtable"/>) using their VR placable name as the key and the on-ship name as the value. Also adds all of the on-ship placed names to the list <see cref="_shipPartsPlacedNames"/>.</br>
    /// </summary>
    public void Start()
    {
        for(int x = 0; x < _shipPartsVR.Length; x++)
        {
            _shipPartsHashtable.Add(_shipPartsVR[x].name, _shipPartsPlaced[x].name);
        }
        // Yes, this needs to be two separate loops. You refactored this in the past and broke it!
        _shipPartsPlacedNames = new string[_shipPartsVR.Length];
        for (int x = 0; x< _shipPartsVR.Length; x++)
        {
            _shipPartsPlacedNames[x] = _shipPartsPlaced[x].name;
        }

    }
    /// <summary>
    /// <br>Returns a <see cref="string"/> with extra instances of "(Clone)" removed from the end of the string.</br>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private string CleanupName(string name)
    {
        if (name.IndexOf("(") == -1) return name;
        else if (name.IndexOf("(Clone)") != -1) return name.Substring(0, name.IndexOf("(Clone)"));
        else return name[(name.IndexOf("("))..];
    }
    /// <summary>
    /// <br>Returns <see cref="Quaternion"/> with <see cref="GameObject"/> 'ob' x, y, z rotation clamped to nearest 90 degrees.</br>
    /// <br>Needs optimization!</br>
    /// </summary>
    /// <param name="ob"></param>
    /// <returns></returns>
    private Quaternion ClampAxisRotation(GameObject ob)
    {
        float x = ((ob.transform.localEulerAngles.x % 360f) +360f) % 360f;
        float y = ((ob.transform.localEulerAngles.y % 360f) + 360f) % 360f;
        float z = ((ob.transform.localEulerAngles.z % 360f) + 360f) % 360f;

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
        return Quaternion.Euler(x, y, z);
    }
    /// <summary>
    /// <br>Returns a <see cref="Vector3"/> that is the <see cref="GameObject"/>'s local coordinates rounded to the nearest whole number as a <see cref="float"/>.</br>
    /// </summary>
    /// <param name="ob"></param>
    /// <returns></returns>
    private Vector3 ClampPosition(GameObject ob)
    {
        return new Vector3((float)Math.Round(ob.transform.localPosition.x), (float)Math.Round(ob.transform.localPosition.y), (float)Math.Round(ob.transform.localPosition.z));
    }
    /// <summary>
    /// <br>Logs <see cref="GameObject"/>'s local and global rotation to console.</br>
    /// <br><paramref name="endStr"/> is an optional <see cref="string"/> to add to the end of the log, such as "after clamp" or "test method abc."</br>
    /// </summary>
    /// <param name="ob"></param>
    private void DebugRotation(GameObject ob, string endStr = "")
    {
        Debug.Log($"Local rotation of {ob.name} x:{ob.transform.localEulerAngles.x} y:{ob.transform.localEulerAngles.y} z:{ob.transform.localEulerAngles.z} {endStr}");
        Debug.Log($"Global rotation of {ob.name} x:{ob.transform.eulerAngles.x} y:{ob.transform.eulerAngles.y} z:{ob.transform.eulerAngles.z} {endStr}");
    }
    /// <summary>
    /// <br>Logs <see cref="GameObject"/>'s local and global position to console.</br>
    /// <br><paramref name="endStr"/> is an optional <see cref="string"/> to add to the end, such as "after clamp" or "test method abc."</br>
    /// </summary>
    /// <param name="ob"></param>
    private void DebugPosition(GameObject ob, string endStr = "")
    {
        Debug.Log($"Local position of {ob.name} x:{ob.transform.localPosition.x} y:{ob.transform.localPosition.y} z:{ob.transform.localPosition.z} {endStr}");
        Debug.Log($"Global rotation of {ob.name} x:{ob.transform.position.x} y:{ob.transform.position.y} z:{ob.transform.position.z} {endStr}");
    }
    /// <summary>
    /// <br>Returns true if <see cref="_previewObject"/> exists and none of the <see cref="Vector3"/> coordinates in <see cref="_shipParts"/> are taken already.</br>
    /// <br>If a <see cref="Vector3"/> coortinate is taken, makes the <see cref="_previewObject"/> <see cref="Material"/> <see cref="Color"/> red and increases the <see cref="Transform.localScale"/> by</br>
    /// <br>1% and returns false.</br>
    /// </summary>
    /// <returns></returns>
    private bool CheckIfValidPlacement()
    {//Needs optimization!
        //eventually change _shipParts to a shipparts object list and check if placement spot is taken
        //how to have this work with multi-sized objects????
        if(_previewObject != null)
        {
            foreach (Vector3 vect in _shipParts)
            {
                if (_previewObject.transform.localPosition == vect)
                {
                    _previewObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
                    _previewObject.transform.localScale *= 1.01f;
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// <br>Instantiates <see cref="_previewObject"/> with the same <see cref="Quaternion"/> as <see cref="_interactableInPlacementField"/> and sets</br>
    /// <br><see cref="_previewObject"/>'s <see cref="Mesh"/> to the same as <see cref="_interactableInPlacementField"/>. Sets the <see cref="Transform.localScale"/> of</br>
    /// <br><see cref="_interactableInPlacementField"/> to 50% and its <see cref="BoxCollider"/> to trigger (to prevent physics collision).</br>
    /// <br><see cref="_interactableInPlacementField"/> is the parent and the object is clamped (<see cref="ClampObject(GameObject)"/>).</br>
    /// <br>Also checks if the projected placement is valid. (<see cref="CheckIfValidPlacement"/>).</br>
    /// </summary>
    private void ShowProjectedPlacement()
    {
        _previewObject = Instantiate(_previewPlacementObject, _interactableInPlacementField.transform.position, _interactableInPlacementField.transform.rotation);
        _previewObject.GetComponent<MeshFilter>().sharedMesh = _interactableInPlacementField.GetComponent<MeshFilter>().sharedMesh;
        _interactableInPlacementField.transform.localScale *= 0.5f;
        _interactableInPlacementField.GetComponent<BoxCollider>().isTrigger = true;
        _previewObject.transform.parent = _interactableInPlacementField.transform;
        ClampObject(_previewObject);
        CheckIfValidPlacement();
    }
    /// <summary>
    /// <br><see cref="GameObject"/> <paramref name="ob"/>'s parent is set to this <see cref="GameObject"/> and its position and rotation are clamped.</br>
    /// </summary>
    /// <param name="ob"></param>
    private void ClampObject(GameObject ob)
    {
        ob.transform.parent = gameObject.transform;
        ob.transform.localRotation = ClampAxisRotation(ob);
        ob.transform.localPosition = ClampPosition(ob);
    }
    /// <summary>
    /// <br>Sets <see cref="_inPlacementField"/> to true once a <see cref="Collider"/> enters, then saves the colliding <see cref="GameObject"/> reference</br>
    /// <br>to <see cref="_interactableInPlacementField"/> then cleans up its name (<see cref="CleanupName(string)"/>).</br>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        _inPlacementField = true;
        _interactableInPlacementField = other.gameObject;
        _interactableInPlacementField.name = CleanupName(_interactableInPlacementField.name);
    }
    /// <summary>
    /// <br>Sets <see cref="_inPlacementField"/> to false once a <see cref="Collider"/> leaves, then sets the colliding <see cref="GameObject"/> reference</br>
    /// <br>to null.</br>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        _inPlacementField = false;
        _interactableInPlacementField = null;
    }
    /// <summary>
    /// <br>Adds the <see cref="Vector3"/> <see cref="List{T}"/> <see cref="_shipPartsToAdd"/> to the <see cref="Vector3"/> <see cref="List{T}"/> <see cref="_shipParts"/></br>
    /// <br>then clears the <see cref="_shipPartsToAdd"/> <see cref="List{T}"/>.</br>
    /// </summary>
    private void RefreshShipParts()
    {
        _shipParts.AddRange(_shipPartsToAdd);
        _shipPartsToAdd.Clear();
    }

    private void FixedUpdate()
    {
        if (_previewObject != null && _interactableInPlacementField.tag == _buildTag)//is != null necessary?
        {
            _interactableInPlacementField.transform.localScale = _localScale;
            _interactableInPlacementField.GetComponent<BoxCollider>().isTrigger = false;
            Destroy(_previewObject);
        }
        if (_interactableInPlacementField != null && _interactableInPlacementField.tag != null && _interactableInPlacementField.GetComponent<XRGrabInteractable>() != null && _shipPartsHashtable.ContainsKey(_interactableInPlacementField.name))
        {
            if (_inPlacementField && _interactableInPlacementField.GetComponent<XRGrabInteractable>().isSelected && _interactableInPlacementField.tag == _buildTag)
            {
                _localScale = _interactableInPlacementField.transform.localScale;
                ShowProjectedPlacement();
            }
            else if (CheckIfValidPlacement() && _inPlacementField && !_interactableInPlacementField.GetComponent<XRGrabInteractable>().isSelected)
            {
                if(_interactableInPlacementField != null)
                {
                    foreach (Vector3 vect in _shipParts)
                    {
                        if (!_placedInThisUpdate && CheckIfValidPlacement())
                        {
                            GameObject placedObject = Instantiate(_shipPartsPlaced[Array.IndexOf(_shipPartsPlacedNames, _shipPartsHashtable[_interactableInPlacementField.name])], _interactableInPlacementField.transform.position, _interactableInPlacementField.transform.rotation);
                            placedObject.GetComponent<MeshFilter>().sharedMesh = _interactableInPlacementField.GetComponent<MeshFilter>().sharedMesh;
                            ClampObject(placedObject);
                            GameObject respawnedObject = Instantiate(_interactableInPlacementField.gameObject, blockRespawnPoint, _interactableInPlacementField.gameObject.transform.rotation);
                            _shipPartsToAdd.Add(placedObject.transform.localPosition);
                            Destroy(_interactableInPlacementField.gameObject);
                            _interactableInPlacementField = null;
                            Destroy(_previewObject);
                            _previewObject = null;
                            _placedInThisUpdate = true;
                        }
                    }
                }
            }
        }
        _placedInThisUpdate = false;
        RefreshShipParts();
    }
}

/*
 * Major issues:
 * 
 * rare ability to still place multiple blocks at same time
 * buggy on edges of the placement field
 * 
 * 
 * 
 * 
 * 
 */