using UnityEngine;

public class WordManager : MonoBehaviour
{
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
        foreach (Word w in words)
        {
            Debug.Log(w.word);
            Debug.Log(w.Phonetics[0].text);
            Debug.Log(w.Meanings[0].Definitions[0].text);
        }
    }
}