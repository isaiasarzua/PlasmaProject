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

    private void GenerateWords()
    {
        StartCoroutine(APIController.instance.GetWords((value) =>
        {
            GenerateDefinitions(value);
        }));
    }

    private void GenerateDefinitions(string[] arr)
    {
        StartCoroutine(APIController.instance.GetDefinition(arr, (value) =>
        {
            GetWordObj(value);
        }));
    }

    // Build Word objects
    private void GetWordObj(Word[] words)
    {
        for (int i = 0; i < words.Length; i++)
        {
            Debug.Log(words[i].word + ": " + words[i].Meanings[0].Definitions[0].text);

            WordUI newWordUI = Instantiate(wordPrefab, container.transform);

            newWordUI.word = words[i];
            newWordUI.DisplayUI();

            //newWordUI.GetComponent<Drag>().SetStartPos();

            // Set event to display word's definition
            newWordUI.GetComponent<Drag>().beginDrag += () => { DefinitionUI.instance.Display(newWordUI.word.Meanings[0].Definitions[0].text); };

            //newWordUI.GetComponent<Drag>().dragEvent += () => { AudioManager.instance.PlayPickupSFX(); };
            //newWordUI.GetComponent<Drag>().dragEvent += () => { CursorManager.instance.PlayPickupSFX(); };

            // Set tag for DropArea to check
            newWordUI.tag = newWordUI.word.Meanings[0].PartOfSpeech;
        }
        Debug.Log("Finished looping");
        //container.enabled = false;
    }
}