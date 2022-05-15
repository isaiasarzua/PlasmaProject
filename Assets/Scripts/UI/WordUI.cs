using UnityEngine;
using UnityEngine.UI;

public class WordUI : MonoBehaviour
{
    public Word word { get; set; }
    [SerializeField]
    private Text DisplayWordText;

    public void DisplayUI()
    {
        DisplayWordText.text = word.word;
    }
}