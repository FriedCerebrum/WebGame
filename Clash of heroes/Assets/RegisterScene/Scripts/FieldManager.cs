using UnityEngine;
using UnityEngine.UI;

public class PasswordInputField : MonoBehaviour
{
    // ������ �� InputField � ����������
    public InputField inputField;
    // ������ �� Text ��� ����������� �������
    public Text placeholderText;
    // ����� �������, ������� ����� ������ � ����������
    public string placeholder = "Password";

    // ���� ��� ������������ ��������� InputField
    private bool isInputFieldEmpty = true;

    private void Start()
    {
        // ������������� �� ������� InputField ��� ������������ ���������
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        inputField.onEndEdit.AddListener(OnInputEndEdit);

        // �������������� ����� �������
        placeholderText.text = placeholder;
    }

    // �����, ���������� ��� ��������� ������ � InputField
    private void OnInputValueChanged(string text)
    {
        // ���� ����� ������ � ���� �� ������, �������� �������
        if (!string.IsNullOrEmpty(text))
        {
            placeholderText.gameObject.SetActive(false);
            isInputFieldEmpty = false;
        }
        else
        {
            // ���� ����� ������, ���������� �������
            placeholderText.gameObject.SetActive(true);
            isInputFieldEmpty = true;
        }
    }

    // �����, ���������� ��� ��������� �������������� InputField
    private void OnInputEndEdit(string text)
    {
        // ���� ����� ������ � ������������ ������� ����� ����, ���������� �������
        if (string.IsNullOrEmpty(text) && !inputField.isFocused)
        {
            placeholderText.gameObject.SetActive(true);
            isInputFieldEmpty = true;
        }
    }
}
