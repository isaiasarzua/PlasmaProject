using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    public void UpdateScore(int i)
    {
        scoreText.text = i + "/16";
    }
}