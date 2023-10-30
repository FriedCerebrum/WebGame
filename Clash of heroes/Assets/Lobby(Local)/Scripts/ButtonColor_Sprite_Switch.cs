using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CustomButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Image image;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public Color pressedColor = Color.gray; // Здесь выбираем оттенок

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            image.sprite = highlightedSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable)
        {
            image.sprite = normalSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
        {
            image.color = pressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable)
        {
            image.color = Color.white;
            image.sprite = normalSprite;
        }
    }
}
