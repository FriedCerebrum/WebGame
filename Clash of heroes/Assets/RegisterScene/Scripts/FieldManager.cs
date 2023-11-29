using UnityEngine;
using UnityEngine.UI;

public class PasswordInputField : MonoBehaviour
{
    // Ссылка на InputField в инспекторе
    public InputField inputField;
    // Ссылка на Text для отображения надписи
    public Text placeholderText;
    // Текст надписи, который можно задать в инспекторе
    public string placeholder = "Password";

    // Флаг для отслеживания состояния InputField
    private bool isInputFieldEmpty = true;

    private void Start()
    {
        // Подписываемся на события InputField для отслеживания изменений
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        inputField.onEndEdit.AddListener(OnInputEndEdit);

        // Инициализируем текст надписи
        placeholderText.text = placeholder;
    }

    // Метод, вызываемый при изменении текста в InputField
    private void OnInputValueChanged(string text)
    {
        // Если текст введен и поле не пустое, скрываем надпись
        if (!string.IsNullOrEmpty(text))
        {
            placeholderText.gameObject.SetActive(false);
            isInputFieldEmpty = false;
        }
        else
        {
            // Если текст пустой, показываем надпись
            placeholderText.gameObject.SetActive(true);
            isInputFieldEmpty = true;
        }
    }

    // Метод, вызываемый при окончании редактирования InputField
    private void OnInputEndEdit(string text)
    {
        // Если текст пустой и пользователь убирает выбор поля, показываем надпись
        if (string.IsNullOrEmpty(text) && !inputField.isFocused)
        {
            placeholderText.gameObject.SetActive(true);
            isInputFieldEmpty = true;
        }
    }
}
