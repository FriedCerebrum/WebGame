using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject objectToDisable;
    public GameObject objectToEnable;

    void Update()
    {
        // ���������, ��� �� ����� ������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ��������� ���� ������ � �������� ������
            objectToDisable.SetActive(false);
            objectToEnable.SetActive(true);

            // ������� ��������� � ������� ��� �������
            Debug.Log("ToggleObjects script has been triggered by pressing Space");

            // ��������� ���� ������
            this.enabled = false;
        }
    }
}
