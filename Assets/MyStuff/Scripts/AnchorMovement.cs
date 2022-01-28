using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorMovement : MonoBehaviour
{
    [SerializeField] private float localXLimitPos;
    [SerializeField] private float localYLimitPos;
    [SerializeField] private float localZLimitPos;
    [SerializeField] private float localXLimitNeg;
    [SerializeField] private float localYLimitNeg;
    [SerializeField] private float localZLimitNeg;
    private float x, y, z;

    private void FixedUpdate()
    {
        if (transform.localPosition.x > localXLimitPos) x = localXLimitPos;
        if (transform.localPosition.x < localXLimitNeg) x = localXLimitNeg;
        if (transform.localPosition.y > localYLimitPos) y = localYLimitPos;
        if (transform.localPosition.y < localYLimitNeg) y = localYLimitNeg;
        if (transform.localPosition.z > localZLimitPos) z = localZLimitPos;
        if (transform.localPosition.z < localZLimitNeg) z = localZLimitNeg;
        if (x != 0f || y != 0f || z != 0f) transform.localPosition = new Vector3(x, y, z);
        x=0f; y=0f; z=0f;
    }
}
