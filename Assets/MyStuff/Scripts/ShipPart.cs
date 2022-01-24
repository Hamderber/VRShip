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
    public GameObject respawnAnchor;
    public GameObject buildingPallet;
    private int count = 0;
    [SerializeField] private GameObject _placementPreview;
    private Vector3 refPos;


    private void Awake()
    {
        _console = GameObject.FindGameObjectWithTag("Console").GetComponent<Debugger>();
        defaultLocalScale = transform.localScale;
        
        RefreshMesh();
        ResetRespawnAnchor();
        refPos = transform.position;
    }

    public void DebugShipPart()
    {
        _console.Log(_debugThisScript, message: $"{name} with scale {defaultLocalScale} and mesh index of {selectedArmorMeshIndex}");
    }


    public void RefreshMesh()
    {
        gameObject.GetComponent<MeshFilter>().sharedMesh = armorPrefabs[selectedArmorMeshIndex].GetComponent<MeshFilter>().sharedMesh;
        DebugShipPart();
    }

    public void ChangeArmorMesh(int delta = 0, int newSelectedIndex = -1)
    {
        selectedArmorMeshIndex = (selectedArmorMeshIndex + delta + armorPrefabs.Length) % armorPrefabs.Length;
        if (newSelectedIndex != -1) selectedArmorMeshIndex = newSelectedIndex;
        RefreshMesh();
    }

    public void ResetRespawnAnchor()
    {
        if (!gameObject.GetComponent<XRGrabInteractable>().isSelected && transform.position != refPos)
        {
            transform.parent = respawnAnchor.transform.parent;
            transform.localPosition = respawnAnchor.transform.localPosition;
            transform.parent = null;
            transform.rotation = buildingPallet.transform.rotation;
            _console.Log(_debugThisScript, message: $"{name} respawn anchor reset to {respawnAnchor.name} ({respawnAnchor.transform.localPosition})");
            refPos = transform.position;
        }
    }

    public void TogglePlacementPreview()
    {
        if(gameObject.GetComponent<XRGrabInteractable>().isSelected)
        {
            _placementPreview.SetActive(true);
        }
        else
        {
            _placementPreview.SetActive(false);
        }
    }

    public void FixedUpdate()
    {
        
        TogglePlacementPreview();
        if(!gameObject.GetComponent<XRGrabInteractable>().isSelected) count++;
        if (!gameObject.GetComponent<XRGrabInteractable>().isSelected && count >= 240)
        {
            ResetRespawnAnchor();
            count = 0;
        }
    }
}
