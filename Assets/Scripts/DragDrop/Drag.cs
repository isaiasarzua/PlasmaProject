// Based on Unity's drag inferface: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDragHandler.html
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform dragObject;
    public CanvasGroup canvasGroup;
    private Vector3 startPos;


    // We want to know when a word is dragged so we can display it's definition
    public StringEvent DragEvent = new StringEvent();


    // For DropArea to let us know object has been dropped
    private bool dropped;
    public bool Dropped
    {
        get { return dropped; }
        set { dropped = value; }
    }

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
        // Response to beginning drag: Change color of drag object or cursor, sfx, etc.
        // soundManager.PlayDragSFX();


        if (DragEvent != null)
        {
            DragEvent.Invoke("test");
        }

        // Must block raycasts so that DropArea can detect OnDrop from IDropHandler
        canvasGroup.blocksRaycasts = false;

        // Reset 'dropped' property so we can reset position OnEndDrag
        Dropped = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Response to ending drag: Change color of drag object or cursor, sfx, etc.
        // soundManager.PlayDropSFX();

        // Reset raycasts
        canvasGroup.blocksRaycasts = true;

        // Reset position if object was not placed in a DropArea
        if (!Dropped) dragObject.position = startPos;
    }
}