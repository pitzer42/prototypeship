using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float angularSpeed;
    public bool lockXZ = false;
    private Vector3 relativePosition;
    private Quaternion relativeRotation;
    private Vector3 originalEulerAngles;

    void Start()
    {
        relativePosition = transform.position - target.position;
        relativeRotation = transform.rotation * target.rotation;
        originalEulerAngles = transform.rotation.eulerAngles;
    }
    
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + relativePosition, Time.deltaTime * speed);
    }

    void OldLateUpdate()
    {
        Vector3 position = target.TransformPoint(relativePosition);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);

        Quaternion rotation = target.rotation * relativeRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * angularSpeed);
        if (lockXZ)
        {
            Vector3 rot = transform.eulerAngles;
            rot.z = originalEulerAngles.z;
            rot.x = originalEulerAngles.x;
            transform.eulerAngles = rot;
        }
    }
}
