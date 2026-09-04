using UnityEngine;

public class ScoreTransfer : MonoBehaviour
{
    public static ScoreTransfer Instance;

    public int player1Score;
    public int player2Score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetScores(int player1, int player2)
    {
        player1Score = player1;
        player2Score = player2;

        Debug.Log(
            $"[ScoreTransfer] Final Scores | " +
            $"Player 1: {player1Score} | " +
            $"Player 2: {player2Score}"
        );
    }
}