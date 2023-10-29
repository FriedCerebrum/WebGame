using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeadScript : MonoBehaviour
{
    public HealthTest healthTest;
    public GameObject targetObject;
    public float forceAmount = 1500f;
    public Rigidbody2D rb;
    bool forced = false;
    public void Update()
    {
        if (healthTest != null)
        {
            if (healthTest.healthSlider.value <= 0.1f && !forced)
            {
                Debug.Log("Умерли от причины 1");
                ApplyForceToTarget();
                forced = true;
            }
        }
        if (healthTest == null && !forced)
        {
            Debug.Log("Умерли от причины 2");
            ApplyForceToTarget();
            forced = true;
        }
    }
    public void ApplyForceToTarget()
    {
        if (rb != null)
        {
            rb.AddForce(new Vector2(0f, forceAmount));
        }
    }
}
