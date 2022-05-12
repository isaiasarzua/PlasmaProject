// Based on Unity's drop interface :https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDropHandler.html
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Object is in drop area");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            eventData.pointerDrag.GetComponent<Drag>().Dropped = true;
        }
    }
}