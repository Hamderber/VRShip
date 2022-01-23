using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ShipPart : MonoBehaviour
{
    private bool _debugThisScript = true;
    private Debugger _console;

    public GameObject[] armorPrefabs;
    public int selectedArmorMeshIndex = 0;
    public List<Vector3> localPlacementCoordinates = new();
    public Vector3 blockRespawnPoint = Vector3.zero;
    public Vector3 defaultLocalScale = Vector3.one;


    private void Awake()
    {
        defaultLocalScale = transform.localScale;
        RefreshMesh();
    }
    private void OnDestroy()
    {
        //not working because sometimes called after script cleared from memory somehow?
        //_console.Log(_debugThisScript, message: $"Destroy effect TODO!");
    }


    public void RefreshMesh()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[selectedArmorMeshIndex].GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeArmorMesh(int delta = 0, int newSelectedIndex = -1)
    {
        selectedArmorMeshIndex = (selectedArmorMeshIndex + delta + armorPrefabs.Length) % armorPrefabs.Length;
        if (newSelectedIndex != -1) selectedArmorMeshIndex = newSelectedIndex;
        RefreshMesh();
    }
}
