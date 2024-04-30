using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private ParticleSystem collectionParticles;
    private float destroyDelay = 0.5f;

    private void Start()
    {
        collectionParticles = GetComponentInChildren<ParticleSystem>();
    }
    public void Collect()
    {
        GameManager.Instance.AddGem();
        collectionParticles.Play();
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        Destroy(gameObject, destroyDelay);
    }
}
