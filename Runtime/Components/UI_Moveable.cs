using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CrossingLears
{
public class UI_Moveable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float AlphaOnDrag = 1;
    
    [Tooltip("The moving object that is being clamped")]
    public RectTransform ContentRectTransform;
    [Tooltip("Parent of ContentRectTransform, used for clamping")]
    [SerializeField] private RectTransform parentRectTransform;
    public RectOffset Padding = new();

    public UnityEvent onBeginDrag;
    public UnityEvent onEndDrag;


    private CanvasGroup canvasGroup;
    private Vector2 pointerOffset;

    private float originalAlpha;

    void Start()
    {
        if(ContentRectTransform == null)
            ContentRectTransform = transform as RectTransform;

        parentRectTransform = ContentRectTransform.parent as RectTransform;
        if(parentRectTransform == null)
        {
            Debug.LogError("parentRectTransform must not be null");
        }

        if (AlphaOnDrag != 1)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if(!canvasGroup) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag.Invoke();
        if(canvasGroup)
        {
            originalAlpha = canvasGroup.alpha;
            canvasGroup.alpha = AlphaOnDrag;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ContentRectTransform, eventData.position, eventData.pressEventCamera, out pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            ContentRectTransform.localPosition = localPointerPosition - pointerOffset;
            ClampToBounds();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canvasGroup)
        {
            canvasGroup.alpha = originalAlpha;
        }
        onEndDrag.Invoke();
    }

    private void ClampToBounds()
    {
        Vector3[] parentCorners = new Vector3[4];
        parentRectTransform.GetWorldCorners(parentCorners);

        Vector3[] objCorners = new Vector3[4];
        ContentRectTransform.GetWorldCorners(objCorners);

        float minX = parentCorners[0].x + Padding.left;
        float maxX = parentCorners[2].x - Padding.right;
        float minY = parentCorners[0].y + Padding.bottom;
        float maxY = parentCorners[2].y - Padding.top;

        Vector3 position = ContentRectTransform.position;

        float width = objCorners[2].x - objCorners[0].x;
        float height = objCorners[2].y - objCorners[0].y;

        position.x = Mathf.Clamp(position.x, minX + width / 2, maxX - width / 2);
        position.y = Mathf.Clamp(position.y, minY + height / 2, maxY - height / 2);

        ContentRectTransform.position = position;
    }
    }
}
