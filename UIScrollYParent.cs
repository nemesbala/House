using UnityEngine;
using UnityEngine.EventSystems;

public class UIScrollYParent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IScrollHandler
{
    public RectTransform contentToMove;
    public float minY = -500f;
    public float maxY = 500f;
    public float scrollSpeed = 20f;
    public float smoothingSpeed = 10f;

    private Vector2 startMousePosition;
    private Vector2 startContentPosition;
    private bool isDragging = false;
    private float targetY;

    void Awake()
    {
        if (contentToMove == null)
            contentToMove = GetComponent<RectTransform>();

        targetY = contentToMove.anchoredPosition.y;
    }

    void Update()
    {
        Vector2 current = contentToMove.anchoredPosition;
        current.y = Mathf.Lerp(current.y, targetY, Time.deltaTime * smoothingSpeed);
        contentToMove.anchoredPosition = current;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(contentToMove.parent as RectTransform, eventData.position, eventData.pressEventCamera, out startMousePosition);
        startContentPosition = contentToMove.anchoredPosition;
        targetY = startContentPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(contentToMove.parent as RectTransform, eventData.position, eventData.pressEventCamera, out currentMousePosition);
        float deltaY = currentMousePosition.y - startMousePosition.y;
        targetY = Mathf.Clamp(startContentPosition.y + deltaY, minY, maxY);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnScroll(PointerEventData eventData)
    {
        targetY += eventData.scrollDelta.y * scrollSpeed;
        targetY = Mathf.Clamp(targetY, minY, maxY);
    }
}
