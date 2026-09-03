using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private int playerId = 1;

    [Header("References")]
    [SerializeField]
    private HitGameplay hitGameplay;

    [SerializeField]
    private Text scoreText;

    [Header("Score Values")]
    [SerializeField]
    private int perfectScore = 1000;

    [SerializeField]
    private int greatScore = 750;

    [SerializeField]
    private int goodScore = 500;

    [SerializeField]
    private int missScore = 0;

    private int score = 0;

    private void Awake()
    {
        if (hitGameplay == null)
        {
            Debug.LogError(
                $"[ScoreUI] HitGameplay is not assigned " +
                $"for Player {playerId}.",
                this
            );
        }

        if (scoreText == null)
        {
            Debug.LogError(
                $"[ScoreUI] Score Text is not assigned " +
                $"for Player {playerId}.",
                this
            );
        }

        UpdateScoreUI();
    }

    private void OnEnable()
    {
        if (hitGameplay != null)
        {
            hitGameplay.HitDetected += OnHitDetected;
        }
    }

    private void OnDisable()
    {
        if (hitGameplay != null)
        {
            hitGameplay.HitDetected -= OnHitDetected;
        }
    }

    private void OnHitDetected(HitResult result)
    {
        // Ignore the other player's hits
        if (result.PlayerId != playerId)
            return;

        int points = GetScoreForJudgement(
            result.Judgement
        );

        score += points;

        UpdateScoreUI();

        Debug.Log(
            $"[ScoreUI] Player {playerId} | " +
            $"Judgement={result.Judgement} | " +
            $"Points=+{points} | " +
            $"Score={score}"
        );
    }

    private int GetScoreForJudgement(
        HitJudgement judgement)
    {
        switch (judgement)
        {
            case HitJudgement.Perfect:
                return perfectScore;

            case HitJudgement.Great:
                return greatScore;

            case HitJudgement.Good:
                return goodScore;

            default:
                return missScore;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null)
            return;

        scoreText.text =
            $"SCORE: {score}";
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}