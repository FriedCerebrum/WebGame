using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthManager2 : MonoBehaviour
{
    public Slider localPlayerSlider;  // ������� ��� ���������� ������
    public Slider enemySlider;        // ������� ��� ����������
    private Entity2 localPlayerEntity;
    private Entity2 enemyEntity;

    void Update()
    {
        if (localPlayerEntity == null || enemyEntity == null)
        {
            FindPlayers();
            return;
        }

        // ��������� ��������
        localPlayerSlider.value = localPlayerEntity.hp;
        enemySlider.value = enemyEntity.hp;
    }

    private void FindPlayers()
    {
        // ������� ��� ������� � ����������� Entity2
        Entity2[] players = FindObjectsOfType<Entity2>();

        foreach (var player in players)
        {
            if (player.photonView.IsMine)
            {
                localPlayerEntity = player;  // ��������� �����
            }
            else
            {
                enemyEntity = player;        // ���������
            }
        }
    }
}
