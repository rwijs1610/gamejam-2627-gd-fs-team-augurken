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

        if (noteLayer.value == 0)
        {
            Debug.LogWarning(
                "[HitGameplay] Note LayerMask is empty."
            );
        }

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

    public bool TryHit(
        PlayerHitbox playerHitbox,
        int playerId,
        int laneId)
    {
        if (playerHitbox == null)
        {
            Debug.LogWarning(
                "[HitGameplay] TryHit received a null PlayerHitbox."
            );

            return false;
        }

        if (_detector == null)
        {
            Debug.LogError(
                "[HitGameplay] HitDetector is not initialized."
            );

            return false;
        }

        if (_accuracy == null)
        {
            Debug.LogError(
                "[HitGameplay] AccuracySystem is not initialized."
            );

            return false;
        }

        if (_score == null)
        {
            Debug.LogError(
                "[HitGameplay] ScoreSystem is not initialized."
            );

            return false;
        }

        if (_streak == null)
        {
            Debug.LogError(
                "[HitGameplay] StreakSystem is not initialized."
            );

            return false;
        }

        if (playerId < 0)
        {
            Debug.LogWarning(
                $"[HitGameplay] Invalid PlayerId: {playerId}"
            );

            return false;
        }

        if (laneId < 0)
        {
            Debug.LogWarning(
                $"[HitGameplay] Invalid LaneId: {laneId}"
            );

            return false;
        }

        bool success = _detector.TryHit(
            playerHitbox,
            playerId,
            laneId,
            out HitResult result
        );

        if (!success)
        {
            _streak.RegisterMiss(
                playerId
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
            string noteName =
                result.Note != null
                    ? result.Note.name
                    : "NULL";

            Debug.Log(
                $"[HitGameplay] HIT\n" +
                $"Player: {result.PlayerId}\n" +
                $"Lane: {result.LaneId}\n" +
                $"Note: {noteName}\n" +
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
        if (_accuracy == null)
        {
            Debug.LogError(
                "[HitGameplay] AccuracySystem is not initialized."
            );

            return 0f;
        }

        return _accuracy.GetAccuracy(playerId);
    }

    public int GetScore(int playerId)
    {
        if (_score == null)
        {
            Debug.LogError(
                "[HitGameplay] ScoreSystem is not initialized."
            );

            return 0;
        }

        return _score.GetScore(playerId);
    }

    public int GetHitCount(int playerId)
    {
        if (_accuracy == null)
        {
            Debug.LogError(
                "[HitGameplay] AccuracySystem is not initialized."
            );

            return 0;
        }

        return _accuracy.GetHitCount(playerId);
    }
}