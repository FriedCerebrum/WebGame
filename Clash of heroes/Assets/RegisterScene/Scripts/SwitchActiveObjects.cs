using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    public void Toggle()
    {
        // ¬ключаем или выключаем объекты в массиве objectsToEnable
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(!obj.activeSelf);
        }

        // ¬ключаем или выключаем объекты в массиве objectsToDisable
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
