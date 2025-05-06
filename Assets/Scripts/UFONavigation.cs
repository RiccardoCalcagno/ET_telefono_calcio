using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFONavigation : MonoBehaviour
{
    [Header("Target")]
    public Transform handAnchor;

    [Header("Rotation Control")]
    public float angularSpeed = Mathf.PI * 0.4f; // 1 giro ogni 5 sec

    private Rigidbody myRigidBody;
    private Vector3 lastTargetPosition;
    private Vector3 targetVelocity;
    private float kp = 50f; // guadagno proporzionale
    private float kd = 20f; // guadagno derivativo
    private float maxForce = 30f;
    private float smoothTime = 0.05f; // tempo di reazione al target



    // Start is called before the first frame update
    void Start()
    {
        if(handAnchor == null)
        {
            throw new System.Exception("Assign an handAnchor");
        }

        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Richiamato ad ogni tick del motore della fisica
    void FixedUpdate()
    {
        // === 1. TARGET SMOOTHING ===
        Vector3 rawTarget = handAnchor.position;
        Vector3 smoothedTarget = Vector3.Lerp(lastTargetPosition, rawTarget, Time.fixedDeltaTime / smoothTime);
        targetVelocity = (smoothedTarget - lastTargetPosition) / Time.fixedDeltaTime;
        lastTargetPosition = smoothedTarget;


        // === 2. Controllo PID (Proporzionale, Integrale, Derivativo)  https://it.wikipedia.org/wiki/Controllo_PID ===
        Vector3 positionError = smoothedTarget - transform.position;
        Vector3 velocityError = targetVelocity - myRigidBody.velocity;

        // Calcolo della forza PID: F = Kp * errore_pos + Kd * errore_vel
        Vector3 controlForce = (kp * positionError) + (kd * velocityError);

        // Clamp della forza per evitare picchi
        if (controlForce.magnitude > maxForce)
            controlForce = controlForce.normalized * maxForce;


        // Azione fisica sulla posizione della navicella
        myRigidBody.AddForce(controlForce, ForceMode.Force);


        // Azione cinetica sulla momento angolare
        myRigidBody.angularVelocity = new Vector3(0f, angularSpeed, 0f);
    }
}
