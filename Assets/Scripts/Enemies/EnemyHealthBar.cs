using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private Camera cam;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
