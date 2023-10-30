using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    public Slider healthSlider;

    private void Start()
    {
        healthSlider.value = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            healthSlider.value -= 0.25f;
        }
    }
}

