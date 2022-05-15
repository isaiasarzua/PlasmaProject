// Based on Unity's drag inferface: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDragHandler.html
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool Dropped { get; set; }
    public CanvasGroup canvasGroup;
    private RectTransform dragObject;
    private Vector3 startPos;


    // Know when a word is dragged so we can display it's definition
    public delegate void BeginDrag();
    public event BeginDrag beginDrag;

    private void Start()
    {
        dragObject = transform as RectTransform;
    }

    public void SetStartPos()
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // anchor drag object to mouse position
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragObject, eventData.position, eventData.pressEventCamera, out var globalMousePos))
        {
            dragObject.position = globalMousePos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (beginDrag != null)
        {
            beginDrag.Invoke();
        }

        // Must block raycasts so that DropArea can detect OnDrop from IDropHandler
        canvasGroup.blocksRaycasts = false;

        // Reset 'dropped' property so we can reset position OnEndDrag
        Dropped = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset raycasts
        canvasGroup.blocksRaycasts = true;

        // Reset position if object was not placed in a DropArea
        if (!Dropped) dragObject.position = startPos;
    }
}