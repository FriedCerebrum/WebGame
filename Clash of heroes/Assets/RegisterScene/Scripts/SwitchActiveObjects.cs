using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    public void Toggle()
    {
        // �������� ��� ��������� ������� � ������� objectsToEnable
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(!obj.activeSelf);
        }

        // �������� ��� ��������� ������� � ������� objectsToDisable
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }
}
