using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioToFinalShowdown : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Scores")]
    [SerializeField] private ScoreUI player1ScoreUI;
    [SerializeField] private ScoreUI player2ScoreUI;

    [Header("Scene")]
    [SerializeField] private string finalShowdownSceneName;

    [Header("Player Won")]
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject player1won;
    [SerializeField] GameObject player2won;

    private bool hasFinished = false;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void Start()
    {
        EndPanel.SetActive(false);
        player2won.SetActive(false);
        player2won.SetActive(false);
    }

    private void Update()
    {
        if (hasFinished)
            return;

        if (audioSource != null &&
            !audioSource.isPlaying &&
            audioSource.clip != null)
        {
            hasFinished = true;
            GoToFinalShowdown();
        }
    }

    private void GoToFinalShowdown()
    {
        if (player1ScoreUI == null)
        {
            Debug.LogError(
                "[AudioToFinalShowdown] Player 1 ScoreUI is not assigned.",
                this
            );
            return;
        }

        if (player2ScoreUI == null)
        {
            Debug.LogError(
                "[AudioToFinalShowdown] Player 2 ScoreUI is not assigned.",
                this
            );
            return;
        }

        int player1Score = player1ScoreUI.GetScore();
        int player2Score = player2ScoreUI.GetScore();

        Debug.Log(
            $"[Final Showdown] " +
            $"Player 1 Score: {player1Score} | " +
            $"Player 2 Score: {player2Score}"
        );

        if (ScoreTransfer.Instance == null)
        {
            Debug.LogError(
                "[AudioToFinalShowdown] No ScoreTransfer exists in the scene.",
                this
            );
            return;
        }

        ScoreTransfer.Instance.SetScores(
            player1Score,
            player2Score
        );
        if (player1Score > player2Score)
        {
            EndPanel.SetActive(true);
            player1won.SetActive(true);
            player2won.SetActive(false);
            Time.timeScale = 0f;
        }
        if (player1Score < player2Score)
        {
            EndPanel.SetActive(true);
            player2won.SetActive(true);
            player1won.SetActive(false);
            Time.timeScale = 0f;
        }
        //SceneManager.LoadScene(finalShowdownSceneName);
        //SceneManager.LoadScene("MainMenu");
    }
}