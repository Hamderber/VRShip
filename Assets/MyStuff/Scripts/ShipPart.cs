using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ShipPart : MonoBehaviour
{
    
    public GameObject[] armorPrefabs;
    public int selectedArmorMeshIndex = 0;
    public int defaultArmorMeshIndex = 0;
    public List<Vector3> localPlacementCoordinates = new();
    public Vector3 blockRespawnPoint = Vector3.zero;


    private void Awake()
    {
        RefreshMesh();
    }
    private void OnDestroy()
    {
        PlayDestroyEffect();
    }

    private void PlayDestroyEffect()
    {
        Debug.Log("Destroy effect");
    }

    public void RefreshMesh()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[selectedArmorMeshIndex].GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeArmorMesh(int delta = 0)
    {
        if (delta == 0)
        {
            RefreshMesh();//mesh sometimes one off for some reason when blocks respawn
        }
        else
        {
            selectedArmorMeshIndex += delta;
            gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[(selectedArmorMeshIndex + delta + armorPrefabs.Length) % armorPrefabs.Length].GetComponent<MeshFilter>().sharedMesh;
            
        }
    }
}
