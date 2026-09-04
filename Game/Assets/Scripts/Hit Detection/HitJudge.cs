public sealed class HitJudge
{
    private readonly float _perfectThreshold;
    private readonly float _greatThreshold;
    private readonly float _goodThreshold;

    public HitJudge(
        float perfectThreshold = 95f,
        float greatThreshold = 75f,
        float goodThreshold = 50f)
    {
        _perfectThreshold = perfectThreshold;
        _greatThreshold = greatThreshold;
        _goodThreshold = goodThreshold;
    }

    public HitJudgement Judge(float accuracyPercent)
    {
        if (accuracyPercent >= _perfectThreshold)
            return HitJudgement.Perfect;

        if (accuracyPercent >= _greatThreshold)
            return HitJudgement.Great;

        if (accuracyPercent >= _goodThreshold)
            return HitJudgement.Good;

        return HitJudgement.Miss;
    }
}