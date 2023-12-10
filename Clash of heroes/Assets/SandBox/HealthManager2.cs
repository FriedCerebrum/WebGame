using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthManager2 : MonoBehaviour
{
    public Slider localPlayerSlider;  // Слайдер для локального игрока
    public Slider enemySlider;        // Слайдер для противника
    private Entity2 localPlayerEntity;
    private Entity2 enemyEntity;

    void Update()
    {
        if (localPlayerEntity == null || enemyEntity == null)
        {
            FindPlayers();
            return;
        }

        // Обновляем слайдеры
        localPlayerSlider.value = localPlayerEntity.hp;
        enemySlider.value = enemyEntity.hp;
    }

    private void FindPlayers()
    {
        // Находим все объекты с компонентом Entity2
        Entity2[] players = FindObjectsOfType<Entity2>();

        foreach (var player in players)
        {
            if (player.photonView.IsMine)
            {
                localPlayerEntity = player;  // Локальный игрок
            }
            else
            {
                enemyEntity = player;        // Противник
            }
        }
    }
}
