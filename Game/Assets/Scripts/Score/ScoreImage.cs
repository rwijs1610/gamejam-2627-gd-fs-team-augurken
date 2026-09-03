using UnityEngine;

public class ScoreImage : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerId = 1;

    [Header("References")]
    [SerializeField] private HitGameplay hitGameplay;
    [SerializeField] private GameObject[] scoreSprites;

    [Header("Lane X Positions")]
    [SerializeField]
    private float[] laneXPositions =
        new float[4];

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
        // Ignore other player's hits
        if (result.PlayerId != playerId)
            return;

        int lane = result.LaneId;

        if (lane < 0 || lane >= 4)
        {
            Debug.LogError(
                $"[ScoreImage] Invalid lane {lane}.",
                this
            );

            return;
        }

        if (laneXPositions == null ||
            laneXPositions.Length < 4)
        {
            Debug.LogError(
                "[ScoreImage] Need 4 lane X positions.",
                this
            );

            return;
        }

        GameObject prefab =
            GetScoreSprite(result.AccuracyPercent);

        if (prefab == null)
        {
            Debug.LogWarning(
                "[ScoreImage] No score sprite found."
            );

            return;
        }

        // Lane determines X
        float x = laneXPositions[lane];

        // Y is shared between all lanes
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

    private GameObject GetScoreSprite(float accuracy)
    {
        if (scoreSprites == null ||
            scoreSprites.Length == 0)
        {
            return null;
        }

        if (accuracy >= perfectThreshold)
        {
            return scoreSprites[0];
        }

        if (accuracy >= greatThreshold)
        {
            if (scoreSprites.Length > 1)
                return scoreSprites[1];
        }

        if (accuracy >= goodThreshold)
        {
            if (scoreSprites.Length > 2)
                return scoreSprites[2];
        }

        if (scoreSprites.Length > 3)
            return scoreSprites[3];

        return scoreSprites[scoreSprites.Length - 1];
    }
}