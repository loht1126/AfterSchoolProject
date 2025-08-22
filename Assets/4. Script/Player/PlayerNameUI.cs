using UnityEngine;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    public TMP_Text playerNameText;

    void Start()
    {
        if (playerNameText != null)
            playerNameText.text = GameSettings.PlayerName;
    }
}
