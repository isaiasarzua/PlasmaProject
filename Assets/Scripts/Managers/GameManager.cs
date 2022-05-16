using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExampleProject.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public int Score { get; set; }
        [SerializeField]
        private ExportData exportData;

        private void Start()
        {
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }

        public void UpdateScore()
        {
            ++Score;
            UIManager.instance.scoreUI.UpdateScore(Score);
        }

        public void SavePlayerName(string text)
        {
            PlayerPrefs.SetString("playerName", text);
        }
        private void SavePlayerScore()
        {
            PlayerPrefs.SetInt("playerScore", Score);
        }

        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void GameOver()
        {
            SavePlayerScore();

            // Play animation in reverse
            UIManager.instance.canvasAnimator.Play("SlideInTransitionReverse");

            // Display gameover panel
            UIManager.instance.DisplayGameoverPanel();

            // Export data
            exportData.ExportTextFile($"{PlayerPrefs.GetString("playerName")} got a score of {PlayerPrefs.GetString("playerScore")}!");
        }
    }
}