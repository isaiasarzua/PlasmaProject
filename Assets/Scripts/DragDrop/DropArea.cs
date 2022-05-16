// Based on Unity's drop interface: https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.IDropHandler.html
using UnityEngine;
using UnityEngine.EventSystems;
using ExampleProject.Managers;

public class DropArea : MonoBehaviour, IDropHandler
{
    public delegate void OnDropEvt(bool correct, Word w);
    public static event OnDropEvt onDropEvt;

    private void Start()
    {
        onDropEvt += UIManager.instance.DropAnswer;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Snap object into position
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = this.GetComponent<RectTransform>().position;
            eventData.pointerDrag.GetComponent<Drag>().Dropped = true;
            eventData.pointerDrag.SetActive(false);
        }

        // Fire Drop event that checks by tag if object is Noun or Adjective
        // Not sure why but this event gets called multiple times, so not currently using it
        //onDropEvt.Invoke(gameObject.CompareTag(eventData.pointerDrag.tag), eventData.pointerDrag.GetComponent<WordUI>().word);

        UIManager.instance.DropAnswer(gameObject.CompareTag(eventData.pointerDrag.tag), eventData.pointerDrag.GetComponent<WordUI>().word);
        WordManager.instance.UpdateWordCount();
    }
}