using UnityEngine;
using UnityEngine.UI;

namespace ExampleProject.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public DefinitionUI definitionDisplay;
        public ScoreUI scoreUI;
        public PlayerNameUI playerUI;
        public Animator canvasAnimator;
        [SerializeField]
        private Text dropAnswer;
        [SerializeField]
        private GameObject gameoverPanel;

        private void Start()
        {
            instance = this;

            // If not null we are in MainGame scene
            if (canvasAnimator != null)
            {
                canvasAnimator.Play("SlideInTransition");
            }
        }

        // Let the player know if they are correct or not
        public void DropAnswer(bool correct, Word w)
        {
            if (correct)
            {
                dropAnswer.text = $"That is correct!\n{w.word} is a {w.Meanings[0].PartOfSpeech}";
                GameManager.instance.UpdateScore();
            }
            else
                dropAnswer.text = $"That is wrong!\n{w.word} is a {w.Meanings[0].PartOfSpeech}";
        }

        public void DisplayGameoverPanel()
        {
            gameoverPanel.SetActive(true);
        }
    }
}