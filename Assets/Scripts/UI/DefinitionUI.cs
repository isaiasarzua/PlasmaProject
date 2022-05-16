using UnityEngine;
using UnityEngine.UI;


public class DefinitionUI : MonoBehaviour
{
    [SerializeField]
    private Text displayText;

    public void Display(string definition)
    {
        displayText.text = definition;
    }
}