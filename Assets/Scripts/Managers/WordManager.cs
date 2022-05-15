using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public WordUI wordPrefab;
    public GridLayoutGroup container;

    private void Start()
    {
        GenerateWords();
    }

    public void GenerateWords()
    {
        StartCoroutine(APIController.instance.GetWords((value) =>
        {
            Debug.Log($"Recieved {value.Length} nouns");
            GenerateDefinitions(value);
        }));
    }

    public void GenerateDefinitions(string[] arr)
    {
        StartCoroutine(APIController.instance.GetDefinition(arr, (value) =>
        {
            Debug.Log($"Recieved {value.Length} definitions");
            GetWordObj(value);
        }));
    }

    // Build Word objects
    public void GetWordObj(Word[] words)
    {
        Debug.Log("Got " + words.Length + " words.");
        for (int i = 0; i < words.Length; i++)
        {
            Debug.Log(words[i].word + ": " + words[i].Meanings[0].Definitions[0].text);

            WordUI newWordUI = Instantiate(wordPrefab, container.transform);

            newWordUI.word = words[i];
            newWordUI.DisplayUI();

            //newWordUI.GetComponent<Drag>().SetStartPos();
            newWordUI.GetComponent<Drag>().DragEvent.AddListener(delegate { DefinitionUI.instance.Display(newWordUI.word.Meanings[0].Definitions[0].text); });
            newWordUI.tag = newWordUI.word.Meanings[0].PartOfSpeech;
        }
        Debug.Log("Finished looping");
        //container.enabled = false;
    }
}