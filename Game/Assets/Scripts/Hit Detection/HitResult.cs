public readonly struct HitResult
{
    public int PlayerId { get; }
    public int LaneId { get; }

    public float AccuracyPercent { get; }

    public HitJudgement Judgement { get; }

    public NoteHitbox Note { get; }

    public HitResult(
        int playerId,
        int laneId,
        float accuracyPercent,
        HitJudgement judgement,
        NoteHitbox note)
    {
        PlayerId = playerId;
        LaneId = laneId;
        AccuracyPercent = accuracyPercent;
        Judgement = judgement;
        Note = note;
    }
}