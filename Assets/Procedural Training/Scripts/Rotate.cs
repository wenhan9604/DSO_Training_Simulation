using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotateSpeed = 0.5f;
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime*rotateSpeed, Space.World);
    }
}
