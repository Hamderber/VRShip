using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ShipPart : MonoBehaviour
{
    public InputActionReference leftTriggerReference;
    public InputActionReference rightTriggerReference;
    public GameObject[] armorPrefabs;
    private int _selectedArmorMeshIndex = 0;
    public int defaultArmorIndex = 0;
    public Vector3 localPlacementCoordinates = Vector3.zero;
    public Vector3 blockRespawnPoint = Vector3.zero;


    private void Awake()
    {
        leftTriggerReference.action.started += LeftTrigger;
        rightTriggerReference.action.started += RightTrigger;
        ChangeArmorMesh(defaultArmorIndex);
    }
    private void OnDestroy()
    {
        leftTriggerReference.action.started -= LeftTrigger;
        rightTriggerReference.action.started -= RightTrigger;
        PlayDestroyEffect();
    }

    private void PlayDestroyEffect()
    {
        Debug.Log("Destroy effect");
    }

    private void ChangeArmorMesh(int delta)
    {
        if (delta == 0) gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[0].GetComponent<MeshFilter>().sharedMesh;
        else
        {
            gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[(_selectedArmorMeshIndex + delta) % armorPrefabs.Length].GetComponent<MeshFilter>().sharedMesh;
            
            _selectedArmorMeshIndex += delta;
        }
        Debug.Log($"Mesh index {(_selectedArmorMeshIndex + delta) % armorPrefabs.Length}");
    }

    private void LeftTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("Left");
        ChangeArmorMesh(-1);
    }

    private void RightTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("Right");
        ChangeArmorMesh(1);
    }


}
