using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectToDisable;
    public GameObject objectToEnable;

    void Update()
    {
        // Проверяем, был ли нажат пробел
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Отключаем один объект и включаем другой
            objectToDisable.SetActive(false);
            objectToEnable.SetActive(true);

            // Выводим сообщение в консоль для отладки
            Debug.Log("ToggleObjects script has been triggered by pressing Space");

            // Отключаем этот скрипт
            this.enabled = false;
        }
    }
}
