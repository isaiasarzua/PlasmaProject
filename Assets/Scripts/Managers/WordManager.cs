using System.Collections.Generic;
using UnityEngine;

namespace ExampleProject.Managers
{
    public class WordManager : MonoBehaviour
    {
        public static WordManager instance;
        public WordUI wordPrefab;
        public Transform container;
        private List<Word> currentWords;

        private int wordCount;

        private void Start()
        {
            instance = this;
            GetWords();
        }
        public bool CheckGameOver { get { return currentWords.Count == 0; } private set { } }

        public void UpdateWordCount()
        {
            wordCount--;
            if (wordCount == 0)
            {
                GameManager.instance.GameOver();
            }
        }

        private void GetWords()
        {
            StartCoroutine(APIController.instance.GetWords((value) => { BuildWordUI(value); }));
        }

        // Build Word objects
        private void BuildWordUI(List<Word> words)
        {
            wordCount = words.Count;

            for (int i = 0; i < words.Count; i++)
            {
                WordUI newWordUI = Instantiate(wordPrefab, container);
                newWordUI.word = words[i];
                newWordUI.DisplayUI();

                // Set event to display word's definition
                newWordUI.GetComponent<Drag>().beginDrag += () => { UIManager.instance.definitionDisplay.Display(newWordUI.word.Meanings[0].Definitions[0].text); };

                // Set tag for DropArea to check
                newWordUI.tag = newWordUI.word.Meanings[0].PartOfSpeech;
            }
            Debug.Log("Finished looping");
        }
    }
}