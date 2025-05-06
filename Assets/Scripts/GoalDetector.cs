using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GoalDetector : MonoBehaviour
{
    private int palloneLayer;

    private void Awake()
    {
        // Cache il layer numerico
        palloneLayer = LayerMask.NameToLayer("Pallone");
        if (palloneLayer == -1)
        {
            Debug.LogError($"Layer Pallone non esiste. Verifica i nomi layer nel progetto.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == palloneLayer)
        {
            Goal(other.gameObject);
        }
    }

    private void Goal(GameObject pallone)
    {
        // 1. Riproduce il suono
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 2. Attiva tutti i ParticleSystem nei figli
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            ps.Play();
        }

        // 3. Avvia la coroutine per spostare il pallone dopo 2.5 secondi
        StartCoroutine(MoveBallAfterDelay(pallone.transform, 2.5f));
    }

    private IEnumerator MoveBallAfterDelay(Transform ballTransform, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Calcolo della direzione locale back (-forward) in world space
        Vector3 localBack = -transform.forward;
        ballTransform.position += localBack * 10f;
    }
}
