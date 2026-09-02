using System;
using UnityEngine;

public sealed class HitGameplay : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField]
    private LayerMask noteLayer;

    [Header("Judgement")]
    [SerializeField]
    [Range(0f, 100f)]
    private float perfectThreshold = 95f;

    [SerializeField]
    [Range(0f, 100f)]
    private float greatThreshold = 75f;

    [SerializeField]
    [Range(0f, 100f)]
    private float goodThreshold = 50f;

    [Header("Debug")]
    [SerializeField]
    private bool debugLogs = true;

    [SerializeField]
    private bool debugRays = true;

    [SerializeField]
    [Min(0f)]
    private float debugRayDuration = 0.5f;

    private HitDetector _detector;
    private AccuracySystem _accuracy;
    private ScoreSystem _score;
    private StreakSystem _streak;

    public event Action<HitResult> HitDetected;

    private void Awake()
    {
        HitJudge judge = new HitJudge(
            perfectThreshold,
            greatThreshold,
            goodThreshold
        );

        _detector = new HitDetector(
            judge,
            noteLayer,
            debugLogs,
            debugRays,
            debugRayDuration
        );

        _accuracy = new AccuracySystem();
        _score = new ScoreSystem();
        _streak = new StreakSystem();

        if (debugLogs)
        {
            Debug.Log(
                "[HitGameplay] READY\n" +
                $"Perfect >= {perfectThreshold}%\n" +
                $"Great >= {greatThreshold}%\n" +
                $"Good >= {goodThreshold}%\n" +
                $"Debug Logs = {debugLogs}\n" +
                $"Debug Rays = {debugRays}"
            );
        }
    }

    public bool TryHit(PlayerHitbox playerHitbox)
    {
        if (playerHitbox == null)
        {
            Debug.LogWarning(
                "[HitGameplay] TryHit received null PlayerHitbox."
            );

            return false;
        }

        bool success = _detector.TryHit(
            playerHitbox,
            out HitResult result
        );

        if (!success)
        {
            _streak.RegisterMiss(
                playerHitbox.PlayerId
            );

            return false;
        }

        _accuracy.RegisterHit(result);
        _score.RegisterHit(result);
        _streak.RegisterHit(result);

        float totalAccuracy =
            _accuracy.GetAccuracy(
                result.PlayerId
            );

        int totalScore =
            _score.GetScore(
                result.PlayerId
            );

        if (debugLogs)
        {
            Debug.Log(
                $"[HitGameplay] HIT\n" +
                $"Player: {result.PlayerId}\n" +
                $"Lane: {result.LaneId}\n" +
                $"Note: {result.Note.name}\n" +
                $"Hit Accuracy: {result.AccuracyPercent:F2}%\n" +
                $"Judgement: {result.Judgement}\n" +
                $"Total Accuracy: {totalAccuracy:F2}%\n" +
                $"Score: {totalScore}"
            );
        }

        HitDetected?.Invoke(result);

        return true;
    }

    public float GetAccuracy(int playerId)
    {
        return _accuracy.GetAccuracy(playerId);
    }

    public int GetScore(int playerId)
    {
        return _score.GetScore(playerId);
    }

    public int GetHitCount(int playerId)
    {
        return _accuracy.GetHitCount(playerId);
    }
}