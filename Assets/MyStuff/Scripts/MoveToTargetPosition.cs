using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetPosition : MonoBehaviour
{
    public float speed = 1f;
    public Transform target;


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
    }
}
