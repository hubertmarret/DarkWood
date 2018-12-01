using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public GameObject target;
    public float rotateSpeed = 5;

    public float MaxScalarBottom = 0.1f;
    public float MaxScalarTop = -0.9f;

    void LateUpdate()
    {
        transform.RotateAround(target.transform.position, transform.up, Input.GetAxis("Mouse X") * rotateSpeed);

        Vector3 positionCam = transform.position;
        Quaternion rotationCam = transform.rotation;

        float verticalRotation = -Input.GetAxis("Mouse Y") * rotateSpeed;
        transform.RotateAround(target.transform.position, transform.right, verticalRotation);

        float scalar = Vector3.Dot(Vector3.up, transform.forward);

        if (scalar > MaxScalarBottom || scalar < MaxScalarTop)
        {
            transform.position = positionCam;
            transform.rotation = rotationCam;
        }

        transform.LookAt(target.transform);
    }
}
