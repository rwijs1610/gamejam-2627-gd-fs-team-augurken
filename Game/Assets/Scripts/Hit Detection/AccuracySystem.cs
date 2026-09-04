using System.Collections.Generic;

public sealed class AccuracySystem
{
    private readonly Dictionary<int, float> _totalAccuracy = new();
    private readonly Dictionary<int, int> _hitCount = new();

    public void RegisterHit(HitResult result)
    {
        int playerId = result.PlayerId;

        if (!_totalAccuracy.ContainsKey(playerId))
            _totalAccuracy[playerId] = 0f;

        if (!_hitCount.ContainsKey(playerId))
            _hitCount[playerId] = 0;

        _totalAccuracy[playerId] +=
            result.AccuracyPercent;

        _hitCount[playerId]++;
    }

    public float GetAccuracy(int playerId)
    {
        if (!_hitCount.TryGetValue(playerId, out int count))
            return 0f;

        if (count == 0)
            return 0f;

        return _totalAccuracy[playerId] / count;
    }

    public int GetHitCount(int playerId)
    {
        return _hitCount.TryGetValue(
            playerId,
            out int count)
            ? count
            : 0;
    }
}