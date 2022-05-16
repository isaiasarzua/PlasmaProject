using UnityEngine;
using UnityEngine.UI;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText;

    private void Start()
    {
        playerNameText.text = PlayerPrefs.GetString("playerName", "Student");
    }
}