using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public WordUI wordPrefab;
    public Transform container;

    private void Start()
    {
        GetWords();
    }

    private void GetWords()
    {
        StartCoroutine(APIController.instance.GetWords((value) => { BuildWordUI(value); }));
    }

    // Build Word objects
    private void BuildWordUI(List<Word> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            WordUI newWordUI = Instantiate(wordPrefab, container);

            newWordUI.word = words[i];
            newWordUI.DisplayUI();

            // Set event to display word's definition
            newWordUI.GetComponent<Drag>().beginDrag += () => { DefinitionUI.instance.Display(newWordUI.word.Meanings[0].Definitions[0].text); };

            //newWordUI.GetComponent<Drag>().beginDrag += () => { AudioManager.instance.PlayPickupSFX(DefinitionUI.instance.Display(newWordUI.word.Phonetics[0].Audio); };
            //newWordUI.GetComponent<Drag>().beginDrag += () => { CursorManager.instance.PlayPickupSFX(); };

            // Set tag for DropArea to check
            newWordUI.tag = newWordUI.word.Meanings[0].PartOfSpeech;
        }
        Debug.Log("Finished looping");
    }
}