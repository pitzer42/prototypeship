using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeepUp : MonoBehaviour
{
    public float angularAcceleration = 3.6f;
    public float threshould= 3.6f;

    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        if (angle > threshould || body.freezeRotation)
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            Quaternion upCorrection = Quaternion.LookRotation(forward, Vector3.up);
            upCorrection = Quaternion.RotateTowards(transform.rotation, upCorrection, angularAcceleration);
            body.MoveRotation(upCorrection);
            body.freezeRotation = Vector3.Angle(transform.up, Vector3.up) > float.Epsilon;
        }
    }
}
