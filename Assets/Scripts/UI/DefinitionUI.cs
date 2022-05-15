using UnityEngine;
using UnityEngine.UI;

public class DefinitionUI : MonoBehaviour
{
    public static DefinitionUI instance;
    [SerializeField]
    private Text displayText;

    private void Start()
    {
        instance = this;
    }

    public void Display(string definition)
    {
        displayText.text = definition;
    }
}