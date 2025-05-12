using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFONavigation : MonoBehaviour
{
    public Transform eyeCenterAnchor;
    public Transform middleFingerExtremity;
    public Transform palm;


    public float openHandCoeff = 0.13f;
    public float closedHandCoeff = 0f;
    public float maxDistanceUFO = 4f; 


    private float angularSpeed = Mathf.PI * 0.4f;
    private float currentOpennessCoeff;
    private Rigidbody myRigidBody;
    private float responsivnessToPosError = 15f; 
    private float maxForce = 5f;


    void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 delta = middleFingerExtremity.position - palm.position;

        float forwardDistance = Vector3.Dot(delta, palm.forward.normalized);

        Debug.Log(forwardDistance);

        currentOpennessCoeff = (Mathf.Clamp(forwardDistance, closedHandCoeff, openHandCoeff) - closedHandCoeff ) / openHandCoeff;
    }


    void FixedUpdate()
    {
        Vector3 dir = (middleFingerExtremity.position - eyeCenterAnchor.position).normalized;
        Vector3 targetLocal = dir * Mathf.Lerp(0f, maxDistanceUFO, currentOpennessCoeff);
        targetLocal += palm.position;

        Vector3 positionError = targetLocal - transform.position;
        Vector3 controlForce = Vector3.ClampMagnitude(
            responsivnessToPosError * positionError, maxForce);


        myRigidBody.AddForce(controlForce, ForceMode.Force);

        myRigidBody.angularVelocity = new Vector3(0f, angularSpeed, 0f);
    }
}
