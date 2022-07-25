using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLightSourseInDistance : MonoBehaviour
{
    public float disablingDistance = 100f;
    void Update()
    {
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        if (Vector3.Distance(Camera.main.transform.position, transform.position) > disablingDistance)
        {
            if (particles.isPlaying) particles.Stop();                      
        }
        else
        {                      
            if (particles.isStopped) particles.Play();
        }
    }
}
