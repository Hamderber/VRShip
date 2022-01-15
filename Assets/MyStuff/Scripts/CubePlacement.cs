using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CubePlacement : MonoBehaviour
{
    private Hashtable _shipParts = new Hashtable();
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

    public void Start()
    {
        for(int x = 0; x < _shipPartsVR.Length; x++)
        {
            Debug.Log("Adding stuff to hash");
            _shipParts.Add(_shipPartsVR[x].name, _shipPartsPlaced[x].name);
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
        //Debug.Log($" Rotation of {_shipCore.name}: x {_shipCore.transform.localEulerAngles.x} y {_shipCore.transform.localEulerAngles.y} z {_shipCore.transform.localEulerAngles.z} (before)");
        //Debug.Log($" Rotation of {ob.name}: x {rotation.x} y {rotation.y} z {rotation.z} (before)");
        DebugRotation(ob, "before");
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

        //Debug.Log($" Rotation: x {x} y {y} z {z} (after)");
        DebugRotation(ob, "after");
        return Quaternion.Euler(x, y, z);
    }

    private Vector3 ClampPosition(GameObject ob)
    {
        /*float x = localPosition.x;
        float y = localPosition.y;
        float z = localPosition.z;
        //Debug.Log($" Local position: x {x} y {y} z {z} (before)");
        //Debug.Log($" Local position: x {(float)Math.Round(x)} y {(float)Math.Round(y)} z {(float)Math.Round(z)} (after)");
        return new Vector3((float)Math.Round(x), (float)Math.Round(y), (float)Math.Round(z));

        //Debug.Log($" Rotation of {_shipCore.name}: x {_shipCore.transform.localEulerAngles.x} y {_shipCore.transform.localEulerAngles.y} z {_shipCore.transform.localEulerAngles.z} (before)");
        //Debug.Log($" Rotation of {ob.name}: x {rotation.x} y {rotation.y} z {rotation.z} (before)");*/

        DebugPosition(ob, "before");
        float x = ob.transform.localPosition.x;
        float y = ob.transform.localPosition.y;
        float z = ob.transform.localPosition.z;
        
        x = (float)Math.Round(x);
        y = (float)Math.Round(y);
        z = (float)Math.Round(z);
        DebugPosition(ob, "after");
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
    private void OnTriggerEnter(Collider other)
    {
        string name = CleanupName(other.gameObject.name);
        //Checks if the colliding object has the build tag that was defined in the editor,
        //Checks if the colliding object is contained in the hashtable by comparing the name of the object (removing the " (" and onwards from
        //the name as a way to check if it is a prefab
        if (other.tag == _buildTag && _shipParts.ContainsKey(name))
        {
            
            //Quaternion rotation = ClampAxisRotation(/*other.gameObject.transform.rotation.eulerAngles*/other.gameObject.transform.eulerAngles/*other.gameObject.transform.InverseTransformDirection(Vector3.forward)*/, other.gameObject);
            Vector3 position = other.transform.position;//ClampPosition(other.transform.position);



            GameObject placedObject = Instantiate(_shipPartsPlaced[Array.IndexOf(_shipPartsPlacedString, _shipParts[name])], position, other.transform.rotation);
            placedObject.transform.parent = gameObject.transform;

            placedObject.transform.localRotation = ClampAxisRotation(placedObject);
            DebugRotation(_shipCore);
            DebugRotation(placedObject, "after clamp");

            placedObject.transform.localPosition = ClampPosition(placedObject);
            DebugPosition(placedObject, "after clamp");

            GameObject respawnedObject = Instantiate(other.gameObject, blockRespawnPoint, other.gameObject.transform.rotation);
            Destroy(other.gameObject);

            //DEBUG
            /*for (int x = 0; x < _shipPartsVR.Length; x++)
            {
                Debug.Log($"Index[{x}] Prefab \"{_shipPartsVR[x].name}\" and Prefab \"{_shipPartsPlaced[x].name}\" and String array \"{_shipPartsPlacedString[x]}\" and hashtable key \"{name}\" returns \"{_shipParts[name]}\"");
            }

            Debug.Log($"The index of \"{_shipParts[name]}\" in array _shipsPartsPlaced is { Array.IndexOf(_shipPartsPlacedString, _shipParts[name])}");*/
        }
    }
}