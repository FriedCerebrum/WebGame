using UnityEngine;
using UnityEngine.UI;

public class HealthTest : MonoBehaviour
{
    public Slider healthSlider;

    private Entity entityScript;

    private void Start()
    {
        healthSlider.value = 1;
        entityScript = GetComponent<Entity>();
    }

    private void Update()
    {
        if (entityScript != null)
        {
            float currentHealth = entityScript.hp;
            float maxHealth = entityScript.maxHp;
            float normalizedHealth = currentHealth / maxHealth;
            healthSlider.value = normalizedHealth;
        }
    }
}
