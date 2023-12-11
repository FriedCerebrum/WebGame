using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // Импорт Photon PUN

public class YellowHealth2 : MonoBehaviourPun
{
    public Slider yellowSlider;
    public float speed = 1.0f;

    public Entity2 entityScript;

    private void Update()
    {
        // Обновляем UI только для локального игрока
        if (photonView.IsMine && entityScript != null)
        {
            float currentHealth = entityScript.hp;
            float maxHealth = entityScript.maxHp;

            float normalizedHealth = currentHealth / maxHealth;
            yellowSlider.value = Mathf.MoveTowards(yellowSlider.value, normalizedHealth, speed * Time.deltaTime);
        }
    }
}
