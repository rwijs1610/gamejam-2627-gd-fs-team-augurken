using System.Collections.Generic;
using UnityEngine;

public sealed class HitDetector
{
    private readonly HitJudge _judge;
    private readonly LayerMask _noteLayer;
    private readonly bool _debugLogs;
    private readonly bool _debugRays;
    private readonly float _debugRayDuration;

    public HitDetector(
        HitJudge judge,
        LayerMask noteLayer,
        bool debugLogs,
        bool debugRays,
        float debugRayDuration)
    {
        _judge = judge;
        _noteLayer = noteLayer;
        _debugLogs = debugLogs;
        _debugRays = debugRays;
        _debugRayDuration = debugRayDuration;

        if (_judge == null)
        {
            Debug.LogError(
                "[HitDetector] HitJudge dependency is null."
            );
        }

        if (_noteLayer.value == 0)
        {
            Debug.LogWarning(
                "[HitDetector] Note LayerMask is empty."
            );
        }
    }

    public bool TryHit(
        PlayerHitbox player,
        int playerId,
        int laneId,
        out HitResult result)
    {
        result = default;

        if (player == null)
        {
            Log(
                "FAILED: PlayerHitbox is null."
            );

            return false;
        }

        if (_judge == null)
        {
            Log(
                "FAILED: HitJudge is null."
            );

            return false;
        }

        if (playerId < 0)
        {
            Log(
                $"FAILED: Invalid PlayerId: {playerId}"
            );

            return false;
        }

        if (laneId < 0)
        {
            Log(
                $"FAILED: Invalid LaneId: {laneId}"
            );

            return false;
        }

        int rayCount =
            player.RayCount;

        if (rayCount <= 0)
        {
            Log(
                $"FAILED: Invalid RayCount on '{player.name}'."
            );

            return false;
        }

        float rayDistance =
            player.GetRayDistance();

        if (rayDistance <= 0f)
        {
            Log(
                $"FAILED: Invalid ray distance on '{player.name}'."
            );

            return false;
        }

        Log(
            $"========== HIT ==========\n" +
            $"Player: {playerId}\n" +
            $"Lane: {laneId}\n" +
            $"Rays: {rayCount}\n" +
            $"Ray Distance: {rayDistance:F3}"
        );

        Dictionary<NoteHitbox, int> noteHits =
            new();

        Dictionary<NoteHitbox, float> noteDistance =
            new();

        for (int i = 0; i < rayCount; i++)
        {
            Ray ray;

            try
            {
                ray = player.GetRay(i);
            }
            catch (System.Exception exception)
            {
                Log(
                    $"FAILED: Could not generate ray {i} | " +
                    $"{exception.Message}"
                );

                continue;
            }

            RaycastHit2D[] hits =
                Physics2D.RaycastAll(
                    ray.origin,
                    ray.direction,
                    rayDistance,
                    _noteLayer
                );

            NoteHitbox validNote = null;

            float closestDistance =
                float.MaxValue;

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                    continue;

                NoteHitbox note =
                    hit.collider.GetComponentInParent<NoteHitbox>();

                if (note == null)
                    continue;

                if (note.IsConsumed)
                    continue;

                if (!note.CompareTag(
                    NoteHitbox.RequiredTag))
                    continue;

                // IMPORTANT:
                // There is no PlayerId or LaneId check here.
                //
                // The pressed PlayerHitbox is already tied
                // to the lane through PlayerLaneInput.
                //
                // The ray only looks for the note in front
                // of that specific hitbox.

                if (hit.distance < closestDistance)
                {
                    closestDistance =
                        hit.distance;

                    validNote =
                        note;
                }
            }

            if (validNote == null)
            {
                DrawRay(
                    ray,
                    rayDistance,
                    Color.red
                );

                Log(
                    $"Ray {i + 1}/{rayCount}: " +
                    $"RED / NO VALID NOTE"
                );

                continue;
            }

            DrawRay(
                ray,
                rayDistance,
                Color.green
            );

            Log(
                $"Ray {i + 1}/{rayCount}: GREEN | " +
                $"Note={validNote.name} | " +
                $"Distance={closestDistance:F3}"
            );

            if (!noteHits.ContainsKey(validNote))
            {
                noteHits[validNote] = 0;
            }

            noteHits[validNote]++;

            if (!noteDistance.ContainsKey(validNote))
            {
                noteDistance[validNote] =
                    closestDistance;
            }
            else
            {
                noteDistance[validNote] =
                    Mathf.Min(
                        noteDistance[validNote],
                        closestDistance
                    );
            }
        }

        if (noteHits.Count == 0)
        {
            Log(
                "FINAL RESULT: NO VALID NOTE HIT"
            );

            Log(
                "=========================="
            );

            return false;
        }

        NoteHitbox bestNote = null;

        int bestHits = -1;

        float bestDistance =
            float.MaxValue;

        foreach (
            KeyValuePair<NoteHitbox, int> pair
            in noteHits)
        {
            NoteHitbox note =
                pair.Key;

            if (note == null)
                continue;

            int hits =
                pair.Value;

            float accuracy =
                (float)hits /
                rayCount *
                100f;

            Log(
                $"Candidate: {note.name} | " +
                $"Hits={hits}/{rayCount} | " +
                $"Accuracy={accuracy:F2}%"
            );

            if (hits > bestHits)
            {
                bestNote = note;
                bestHits = hits;

                bestDistance =
                    noteDistance[note];

                continue;
            }

            if (
                hits == bestHits &&
                noteDistance[note] <
                bestDistance)
            {
                bestNote = note;

                bestDistance =
                    noteDistance[note];
            }
        }

        if (bestNote == null)
        {
            Log(
                "FAILED: No valid note remained."
            );

            return false;
        }

        float finalAccuracy =
            (float)bestHits /
            rayCount *
            100f;

        finalAccuracy =
            Mathf.Clamp(
                finalAccuracy,
                0f,
                100f
            );

        HitJudgement judgement =
            _judge.Judge(
                finalAccuracy
            );

        Log(
            $"FINAL RESULT\n" +
            $"Player: {playerId}\n" +
            $"Lane: {laneId}\n" +
            $"Note: {bestNote.name}\n" +
            $"Rays Hit: {bestHits}/{rayCount}\n" +
            $"Accuracy: {finalAccuracy:F2}%\n" +
            $"Judgement: {judgement}"
        );

        result = new HitResult(
            playerId,
            laneId,
            finalAccuracy,
            judgement,
            bestNote
        );

        if (!bestNote.IsConsumed)
        {
            bestNote.Consume();

            Log(
                $"SUCCESS: {bestNote.name} consumed."
            );
        }
        else
        {
            Log(
                $"WARNING: {bestNote.name} " +
                $"was already consumed."
            );
        }

        Log(
            "=========================="
        );

        return true;
    }

    private void DrawRay(
        Ray ray,
        float distance,
        Color color)
    {
        if (!_debugRays)
            return;

        Debug.DrawRay(
            ray.origin,
            ray.direction * distance,
            color,
            _debugRayDuration
        );
    }

    private void Log(string message)
    {
        if (!_debugLogs)
            return;

        Debug.Log(
            $"[HitDetector]\n{message}"
        );
    }
}