using UnityEngine;

public class ScoreImage : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerId = 1;

    [Header("References")]
    [SerializeField] private HitGameplay hitGameplay;

    [Header("Score Sprites")]
    [SerializeField] private GameObject[] perfectSprites;
    [SerializeField] private GameObject[] greatSprites;
    [SerializeField] private GameObject[] goodSprites;
    [SerializeField] private GameObject[] missSprites;

    [Header("Lane X Positions")]
    [SerializeField]
    private float[] laneXPositions = new float[4];

    [Header("Y Positions")]
    [SerializeField] private float startY = 5f;
    [SerializeField] private float endY = 3f;

    [Header("Movement")]
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private float randomOffset = 0.35f;
    [SerializeField] private float holdDuration = 0.5f;

    [Header("Accuracy")]
    [SerializeField]
    [Range(0f, 100f)]
    private float perfectThreshold = 95f;

    [SerializeField]
    [Range(0f, 100f)]
    private float greatThreshold = 75f;

    [SerializeField]
    [Range(0f, 100f)]
    private float goodThreshold = 50f;

    private void OnEnable()
    {
        if (hitGameplay == null)
        {
            Debug.LogError(
                $"[ScoreImage] HitGameplay is not assigned " +
                $"for Player {playerId}.",
                this
            );

            return;
        }

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

        SpawnScoreImage(
            result.LaneId,
            result.AccuracyPercent
        );
    }

    private void OnMissDetected(
        int missedPlayerId,
        int laneId)
    {
        if (missedPlayerId != playerId)
            return;

        SpawnMissImage(laneId);
    }

    private void SpawnScoreImage(
        int lane,
        float accuracy)
    {
        if (!IsValidLane(lane))
            return;

        GameObject prefab =
            GetScoreSprite(accuracy);

        if (prefab == null)
            return;

        SpawnImage(
            prefab,
            lane
        );
    }

    private void SpawnMissImage(int lane)
    {
        if (!IsValidLane(lane))
            return;

        GameObject prefab =
            GetRandomSprite(missSprites);

        if (prefab == null)
            return;

        SpawnImage(
            prefab,
            lane
        );
    }

    private void SpawnImage(
        GameObject prefab,
        int lane)
    {
        float x =
            laneXPositions[lane];

        Vector3 startPosition =
            new Vector3(
                x,
                startY,
                0f
            );

        Vector3 endPosition =
            new Vector3(
                x,
                endY,
                0f
            );

        GameObject go =
            Instantiate(
                prefab,
                startPosition,
                Quaternion.identity
            );

        if (go == null)
            return;

        ScoreMover mover =
            go.GetComponent<ScoreMover>();

        if (mover == null)
        {
            mover =
                go.AddComponent<ScoreMover>();
        }

        mover.Init(
            startPosition,
            endPosition,
            moveDuration,
            randomOffset,
            holdDuration
        );
    }

    private GameObject GetScoreSprite(
        float accuracy)
    {
        if (accuracy >= perfectThreshold)
        {
            return GetRandomSprite(
                perfectSprites
            );
        }

        if (accuracy >= greatThreshold)
        {
            return GetRandomSprite(
                greatSprites
            );
        }

        if (accuracy >= goodThreshold)
        {
            return GetRandomSprite(
                goodSprites
            );
        }

        return GetRandomSprite(
            missSprites
        );
    }

    private GameObject GetRandomSprite(
        GameObject[] sprites)
    {
        if (sprites == null ||
            sprites.Length == 0)
        {
            return null;
        }

        return sprites[
            Random.Range(
                0,
                sprites.Length
            )
        ];
    }

    private bool IsValidLane(int lane)
    {
        if (lane < 0 || lane >= 4)
        {
            Debug.LogError(
                $"[ScoreImage] Invalid lane {lane}."
            );

            return false;
        }

        if (laneXPositions == null ||
            laneXPositions.Length < 4)
        {
            Debug.LogError(
                "[ScoreImage] Need 4 lane X positions."
            );

            return false;
        }

        return true;
    }
}