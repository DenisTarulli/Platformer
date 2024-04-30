using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerUp : MonoBehaviour
{
    private PowerUps powerUps;
    private float destroyDelay = 0.2f;

    private void Start()
    {
        powerUps = FindObjectOfType<PowerUps>();
    }

    public void PickPwp()
    {
        StartCoroutine(powerUps.AttackUp());
        Destroy(gameObject, destroyDelay);
    }
}
