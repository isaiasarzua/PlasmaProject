// Based on Unity's drop interface: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDropHandler.html
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Snap object into position
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            eventData.pointerDrag.GetComponent<Drag>().Dropped = true;
        }

        // Check by tag if object is Noun or Adjective
        if (eventData.pointerDrag.CompareTag(gameObject.tag))
        {
            Debug.Log(eventData.pointerDrag.GetComponent<Word>().Meanings[0].Definitions[0].text);
            Debug.Log("Correct, this word is a " + gameObject.tag);
        }
        else
            Debug.Log("Incorrect, this word is a " + gameObject.tag);
    }

    public void DisplayWordDefinition()
    {
        // displayText = dataobject.Definition.text;
    }
}