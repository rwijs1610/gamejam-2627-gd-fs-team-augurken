using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private int playerId = 1;

    [Header("References")]
    [SerializeField]
    private HitGameplay hitGameplay;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text multiplierText;

    [Header("Score Values")]
    [SerializeField]
    private int perfectScore = 1000;

    [SerializeField]
    private int greatScore = 750;

    [SerializeField]
    private int goodScore = 500;

    [SerializeField]
    private int missScore = 0;

    [Header("Streak Multiplier")]
    [SerializeField]
    private int hitsPerMultiplier = 10;

    [SerializeField]
    private int maxMultiplier = 5;

    private int score = 0;
    private int streak = 0;
    private int multiplier = 1;

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

        if (hitsPerMultiplier <= 0)
        {
            hitsPerMultiplier = 10;
        }

        if (maxMultiplier < 1)
        {
            maxMultiplier = 1;
        }

        UpdateScoreUI();
    }

    private void OnEnable()
    {
        if (hitGameplay == null)
            return;

        hitGameplay.HitDetected += OnHitDetected;
        hitGameplay.MissDetected += OnMissDetected;
    }

    private void OnDisable()
    {
        if (hitGameplay == null)
            return;

        hitGameplay.HitDetected -= OnHitDetected;
        hitGameplay.MissDetected -= OnMissDetected;
    }

    private void OnHitDetected(HitResult result)
    {
        if (result.PlayerId != playerId)
            return;

        streak++;

        UpdateMultiplier();

        int baseScore =
            GetScoreForJudgement(
                result.Judgement
            );

        int finalScore =
            baseScore * multiplier;

        score += finalScore;

        UpdateScoreUI();

        Debug.Log(
            $"[ScoreUI] Player {playerId} | " +
            $"Streak={streak} | " +
            $"Multiplier=x{multiplier} | " +
            $"BaseScore={baseScore} | " +
            $"Added={finalScore} | " +
            $"Total={score}"
        );
    }

    private void OnMissDetected(
        int missedPlayerId,
        int laneId)
    {
        if (missedPlayerId != playerId)
            return;

        streak = 0;
        multiplier = 1;

        score += missScore;

        UpdateScoreUI();

        Debug.Log(
            $"[ScoreUI] Player {playerId} MISS | " +
            $"Lane={laneId} | " +
            $"Streak reset | " +
            $"Multiplier=x1"
        );
    }

    private void UpdateMultiplier()
    {
        multiplier =
            1 +
            (
                streak /
                hitsPerMultiplier
            );

        multiplier =
            Mathf.Clamp(
                multiplier,
                1,
                maxMultiplier
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
        if (scoreText != null)
        {
            scoreText.text =
                $"SCORE: {score}";
        }

        if (multiplierText != null)
        {
            multiplierText.text =
                $"x{multiplier}";
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetStreak()
    {
        return streak;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public void ResetScore()
    {
        score = 0;
        streak = 0;
        multiplier = 1;

        UpdateScoreUI();
    }
}