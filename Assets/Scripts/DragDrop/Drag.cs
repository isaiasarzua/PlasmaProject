// Based on Unity's drag inferface: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDragHandler.html
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private float speed = 1;

    void Awake()
    {
        // Get rect transform
    }

    public void OnDrag(PointerEventData eventData)
    {
        // anchor drag object to mouse position
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Response to beginning drag: Change color of drag object or cursor, sfx, etc.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Response to ending drag: Change color of drag object or cursor, sfx, etc.
    }
}