using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // ������ Photon PUN

public class HealthTest2 : MonoBehaviourPun
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
        // ��������� �������� ������ ��� ���������� ������
        if (photonView.IsMine && entityScript != null)
        {
            float currentHealth = entityScript.hp;
            float maxHealth = entityScript.maxHp;
            float normalizedHealth = currentHealth / maxHealth;
            healthSlider.value = normalizedHealth;
        }
    }
}
