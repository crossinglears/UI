using UnityEngine;
using UnityEngine.EventSystems;

namespace CrossingLears
{
    
public class UI_Resizable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float AlphaOnDrag = 1;

    public RectTransform TargetRectTransform;
    public ResizeType resizeType = ResizeType.Bottom;

    [Header("Limits")]
    public Vector2 MinimumSize = new Vector2(50, 50);
    public Vector2 MaximumSize = new Vector2(30000, 30000);

    private Vector2 originalSize;
    private Vector2 originalPosition;
    private Vector2 originalMousePosition;

    private CanvasGroup canvasGroup;
    private float originalAlpha;

    public enum ResizeType
    {
        Left,   // Adjust width from the left
        Right,  // Adjust width from the right
        Top,    // Adjust height from the top
        Bottom, // Adjust height from the bottom
    }

    void Start()
    {        
        if (AlphaOnDrag != 1)
        {
            canvasGroup = TargetRectTransform.GetComponent<CanvasGroup>();
            if(!canvasGroup) canvasGroup = TargetRectTransform.gameObject.AddComponent<CanvasGroup>();
        }
    }

    [Button("Current Is Max")]
    public void CurrentIsMax()
    {
        MaximumSize = TargetRectTransform.sizeDelta;
    }

    [Button("Current Is Min")]
    public void CurrentIsMin()
    {
        MinimumSize = TargetRectTransform.sizeDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (TargetRectTransform == null) return;

        // Store initial values
        originalSize = TargetRectTransform.sizeDelta;
        originalPosition = TargetRectTransform.anchoredPosition;
        originalMousePosition = ClampToScreen(eventData.position);

        if(canvasGroup)
        {
            originalAlpha = canvasGroup.alpha;
            canvasGroup.alpha = AlphaOnDrag;
        }       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TargetRectTransform == null) return;

        // clamp mouse position
        Vector2 clampedMousePosition = ClampToScreen(eventData.position);
        Vector2 delta = clampedMousePosition - originalMousePosition;

        Vector2 newSize = originalSize;
        Vector2 newPosition = originalPosition;

        // Adjust size & position based on ResizeType
        switch (resizeType)
        {
            case ResizeType.Left:
                newSize.x = Mathf.Clamp(originalSize.x - delta.x, MinimumSize.x, MaximumSize.x);
                newPosition.x = originalPosition.x + (originalSize.x - newSize.x) / 2f;
                break;

            case ResizeType.Right:
                newSize.x = Mathf.Clamp(originalSize.x + delta.x, MinimumSize.x, MaximumSize.x);
                newPosition.x = originalPosition.x + (newSize.x - originalSize.x) / 2f;
                break;

            case ResizeType.Top:
                newSize.y = Mathf.Clamp(originalSize.y + delta.y, MinimumSize.y, MaximumSize.y);
                newPosition.y = originalPosition.y + (newSize.y - originalSize.y) / 2f;
                break;

            case ResizeType.Bottom:
                newSize.y = Mathf.Clamp(originalSize.y - delta.y, MinimumSize.y, MaximumSize.y);
                newPosition.y = originalPosition.y + (originalSize.y - newSize.y) / 2f;
                break;
        }

        TargetRectTransform.sizeDelta = newSize;
        TargetRectTransform.anchoredPosition = newPosition;
    }

    private Vector2 ClampToScreen(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, 0, Screen.width);
        float clampedY = Mathf.Clamp(position.y, 0, Screen.height);
        return new Vector2(clampedX, clampedY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canvasGroup)
            canvasGroup.alpha = originalAlpha;
    }
}
}