using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragYParent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform parentToMove;
    public float minY = -500f;
    public float maxY = 500f;

    private Vector2 startMousePosition;
    private Vector2 startParentPosition;
    private bool isDragging = false;

    void Awake()
    {
        if (parentToMove == null)
        {
            parentToMove = transform.parent.GetComponent<RectTransform>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentToMove.parent as RectTransform, eventData.position, eventData.pressEventCamera, out startMousePosition);
        startParentPosition = parentToMove.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentToMove.parent as RectTransform, eventData.position, eventData.pressEventCamera, out currentMousePosition);
        float deltaY = currentMousePosition.y - startMousePosition.y;

        float newY = startParentPosition.y + deltaY;
        newY = Mathf.Clamp(newY, minY, maxY);

        parentToMove.anchoredPosition = new Vector2(startParentPosition.x, newY);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}