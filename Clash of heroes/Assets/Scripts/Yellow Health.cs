using UnityEngine;
using UnityEngine.UI;

public class YellowHealth : MonoBehaviour
{
    public Slider yellowSlider;
    public float speed = 1.0f;

    public Entity entityScript;

    private void Update()
    {
        if (entityScript != null)
        {
            float currentHealth = entityScript.hp;
            float maxHealth = entityScript.maxHp;

            float normalizedHealth = currentHealth / maxHealth;

            yellowSlider.value = Mathf.MoveTowards(yellowSlider.value, normalizedHealth, speed * Time.deltaTime);
        }
    }
}


