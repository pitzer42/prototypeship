using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waves;

[RequireComponent(typeof(Rigidbody))]
public class Movement2 : MonoBehaviour
{
    public Buoyancy b;

    public Transform inputReference;

    public float acceleration = 1;
    public float angularAcceleration = 3.6f;
    private Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = ReadInputDirection() * b.Contact;
        Vector3 thrust = Vector3.forward * direction.magnitude * acceleration;
        body.AddRelativeForce(thrust, ForceMode.Acceleration);

        float torqueScale = Vector3.Dot(transform.forward, body.velocity);
	Vector3 torque = Vector3.Cross(transform.forward, direction) * angularAcceleration * torqueScale;
        body.AddTorque(torque, ForceMode.Acceleration);
    }

    private Vector3 ReadInputDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey("w"))
            direction.z += 1;
        if (Input.GetKey("s"))
            direction.z -= 1;
        if (Input.GetKey("d"))
            direction.x += 1;
        if (Input.GetKey("a"))
            direction.x -= 1;
        direction = inputReference.TransformDirection(direction.normalized);
	direction.y = 0;
	return direction.normalized;
    }
}
