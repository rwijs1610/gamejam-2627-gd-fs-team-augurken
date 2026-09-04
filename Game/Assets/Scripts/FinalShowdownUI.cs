using UnityEngine;
using TMPro;

public class FinalShowdownUI : MonoBehaviour
{
    [Header("Score Text")]
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;

    private void Start()
    {
        if (ScoreTransfer.Instance == null)
        {
            Debug.LogError(
                "[FinalShowdownUI] ScoreTransfer was not found."
            );
            return;
        }

        player1ScoreText.text =
            $"PLAYER 1: {ScoreTransfer.Instance.player1Score}";

        player2ScoreText.text =
            $"PLAYER 2: {ScoreTransfer.Instance.player2Score}";
    }
}