using UnityEngine;
using UnityEngine.UI;

public class YellowHealth : MonoBehaviour
{
    public Slider redSlider;
    public Slider yellowSlider;
    public float speed = 1.0f;

    private void Start()
    {
        redSlider.value = yellowSlider.value;
    }

    private void Update()
    {
        yellowSlider.value = Mathf.MoveTowards(yellowSlider.value, redSlider.value, speed * Time.deltaTime);
    }

}

