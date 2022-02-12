using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    private void Start()
    {
        healthBar = GetComponent<Slider>();
    }

    public void UpdateHealth(float fraction)
    {
        healthBar.value = fraction;
    }
}