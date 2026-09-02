using System.Collections.Generic;
using UnityEngine;

public sealed class ScoreSystem
{
    private readonly Dictionary<int, int> _scores = new();

    private const int MaximumScorePerHit = 1000;

    public void RegisterHit(HitResult result)
    {
        int playerId = result.PlayerId;

        if (!_scores.ContainsKey(playerId))
            _scores[playerId] = 0;

        int points =
            Mathf.RoundToInt(
                MaximumScorePerHit *
                (result.AccuracyPercent / 100f)
            );

        _scores[playerId] += points;
    }

    public int GetScore(int playerId)
    {
        return _scores.TryGetValue(
            playerId,
            out int score)
            ? score
            : 0;
    }
}