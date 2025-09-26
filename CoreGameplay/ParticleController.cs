using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public float duration = GameplayVars.DEFAULT_PARTICLE_DURATION;

    void Start()
    {
        Invoke("SelfDestruct", duration);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
