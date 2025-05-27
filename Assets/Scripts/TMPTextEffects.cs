using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPTextEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public TextMeshProUGUI tmpText;
    public float scaleMultiplier = 1.2f;
    public Color hoverColor = Color.red;
    public Color pressedColor = Color.magenta;

    private Vector3 originalScale;
    private FontStyles originalStyle;
    private Color originalColor;

    void Start()
    {
        // Try to get the TextMeshProUGUI component if not assigned
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        // Check if the TextMeshProUGUI component is assigned
        if (tmpText == null)
        {
            Debug.LogError("TMPTextEffects: No TextMeshProUGUI component found.");
            enabled = false;
            return;
        }

        // Stores the original properties!!!
        originalScale = tmpText.transform.localScale;
        originalStyle = tmpText.fontStyle;
        originalColor = tmpText.color; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Apply effects on pointer enter
        tmpText.fontStyle = originalStyle | FontStyles.Underline;
        tmpText.color = hoverColor;
        tmpText.transform.localScale = Vector3.one * scaleMultiplier;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Applies effect on pointer down/press
        tmpText.fontStyle = originalStyle | FontStyles.Underline;
        tmpText.color = pressedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert effects on pointer exit
        tmpText.fontStyle = originalStyle;
        tmpText.color = originalColor;
        tmpText.transform.localScale = originalScale;
    } 
}
