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
            Debug.LogError($"Layer Pallone doesn't exist, please add to project");
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
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            ps.Play();
        }

        StartCoroutine(MoveBallAfterDelay(pallone.transform, 2.5f));
    }

    private IEnumerator MoveBallAfterDelay(Transform ballTransform, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 localBack = -transform.forward;
        ballTransform.position += localBack * 1f;
    }
}
