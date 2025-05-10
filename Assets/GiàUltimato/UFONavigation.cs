using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UFONavigation : MonoBehaviour
{
    [Tooltip("Transform del palmo (root della mano)")]
    public Transform palmo;

    [Tooltip("Transform della punta di una delle dita (es. dito medio)")]
    public Transform puntaDito;

    [Tooltip("Transform della posizione del centro degli occhi")]
    public Transform eyeCenterAnchor;

    [Tooltip("Soglia della dimensine maggiore della mano per considerarla aperta")]
    public float openHandCoeff = 0.13f;

    [Tooltip("Soglia della dimensine maggiore della mano per considerarla aperta")]
    public float closedHandCoeff = 0f;

    [Tooltip("Max distance of the UFO from the eye")]
    public float maxDistanceUFO = 4f; 




    private float angularSpeed = Mathf.PI * 0.4f; // 1 giro ogni 5 sec
    private float currentOpennessCoeff;
    private Rigidbody myRigidBody;
    private float responsivnessToPosError = 15f; 
    private float maxForce = 5f;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Vettore dalla base alla punta
        Vector3 delta = puntaDito.position - palmo.position;

        // Proiezione lungo la direzione dove punta la mano
        float forwardDistance = Vector3.Dot(delta, palmo.forward.normalized);

        Debug.Log(forwardDistance);

        currentOpennessCoeff = (Mathf.Clamp(forwardDistance, closedHandCoeff, openHandCoeff) - closedHandCoeff ) / openHandCoeff;
    }


    // Richiamato ad ogni tick del motore della fisica
    void FixedUpdate()
    {
        Vector3 dir = (puntaDito.position - eyeCenterAnchor.position).normalized;
        Vector3 targetLocal = dir * Mathf.Lerp(0f, maxDistanceUFO, currentOpennessCoeff);
        targetLocal += palmo.position;

        Vector3 positionError = targetLocal - transform.position;
        Vector3 controlForce = Vector3.ClampMagnitude(
            responsivnessToPosError * positionError, maxForce);

        // Azione fisica sulla posizione della navicella
        myRigidBody.AddForce(controlForce, ForceMode.Force);

        // Azione cinetica sulla momento angolare
        myRigidBody.angularVelocity = new Vector3(0f, angularSpeed, 0f);
    }
}
