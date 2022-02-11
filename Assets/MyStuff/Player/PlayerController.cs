using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private XRIDefaultInputActions _XRInput;

    public GameObject menu;

    private void Awake()
    {
        _XRInput = new XRIDefaultInputActions();
    }
    private void Start()
    {
        _XRInput.Player.Menu.performed += Menu;
    }
    private void OnEnable()
    {
        _XRInput.Enable();
    }
    private void OnDisable()
    {
        _XRInput.Disable();
        _XRInput.Player.Menu.performed -= Menu;
    }
    public void Menu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menu.SetActive(!menu.activeSelf);
            Debug.Log($"Menu toggled");
        }

    }
}
