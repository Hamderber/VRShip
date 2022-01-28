using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject shipToControl;
    private float _rotationModifier = 1f;
    public float mass;
    public GameObject consoleObject;

    [SerializeField]
    private bool _isAIControlled = false;

    public int ShipLength = 10;
    public int ShipWidth = 5;
    public int ShipHealth = 100;
    public int ShipShields = 100;
    public List<GameObject> ShipInventory = new List<GameObject>();

    public int TakeDamage(int damage)
    {
        ShipHealth -= damage;
        return ShipHealth;
    }

    public void GenerateRandomInventory()
    {
        // Loot generation for random ships
        if (_isAIControlled)
        {

        }

        // Starter gear for player ships
        else
        {

        }
    }

}
