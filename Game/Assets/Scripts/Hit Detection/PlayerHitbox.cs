using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class PlayerHitbox : MonoBehaviour
{
    [Header("Ray Sampling")]
    [SerializeField]
    [Min(1)]
    private int rayCount = 32;

    [SerializeField]
    private Vector2 rayDirection = Vector2.up;

    [SerializeField]
    [Min(0f)]
    private float rayPadding = 0.05f;

    [Header("Debug")]
    [SerializeField]
    private bool showGizmos = true;

    private BoxCollider2D _collider;

    public int RayCount => rayCount;

    public Vector2 RayDirection
    {
        get
        {
            if (rayDirection.sqrMagnitude < 0.0001f)
                return Vector2.up;

            return rayDirection.normalized;
        }
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
        {
            Debug.LogError(
                $"[PlayerHitbox] BoxCollider2D missing | " +
                $"Object={name}",
                this
            );

            return;
        }

        Debug.Log(
            $"[PlayerHitbox] Initialized | " +
            $"Object={name} | " +
            $"Rays={rayCount} | " +
            $"Direction={RayDirection}"
        );
    }

    public Ray GetRay(int index)
    {
        if (_collider == null)
        {
            throw new System.InvalidOperationException(
                $"[PlayerHitbox] Collider is missing on '{name}'."
            );
        }

        if (index < 0 || index >= rayCount)
        {
            throw new System.ArgumentOutOfRangeException(
                nameof(index)
            );
        }

        Bounds bounds = _collider.bounds;

        Vector2 direction = RayDirection;

        Vector2 perpendicular =
            new Vector2(
                -direction.y,
                direction.x
            );

        float halfRayDepth =
            0.5f *
            (
                Mathf.Abs(direction.x) * bounds.size.x +
                Mathf.Abs(direction.y) * bounds.size.y
            );

        float halfRayWidth =
            0.5f *
            (
                Mathf.Abs(perpendicular.x) * bounds.size.x +
                Mathf.Abs(perpendicular.y) * bounds.size.y
            );

        float normalizedPosition =
            rayCount == 1
                ? 0.5f
                : (float)index / (rayCount - 1);

        float perpendicularOffset =
            Mathf.Lerp(
                -halfRayWidth,
                halfRayWidth,
                normalizedPosition
            );

        Vector2 origin =
            (Vector2)bounds.center
            - direction *
            (halfRayDepth + rayPadding)
            + perpendicular *
            perpendicularOffset;

        return new Ray(
            origin,
            direction
        );
    }

    public float GetRayDistance()
    {
        if (_collider == null)
            return 0f;

        Bounds bounds = _collider.bounds;

        Vector2 direction = RayDirection;

        float halfRayDepth =
            0.5f *
            (
                Mathf.Abs(direction.x) * bounds.size.x +
                Mathf.Abs(direction.y) * bounds.size.y
            );

        return
            (halfRayDepth * 2f) +
            (rayPadding * 2f);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        BoxCollider2D box =
            GetComponent<BoxCollider2D>();

        if (box == null)
            return;

        Vector2 direction =
            rayDirection.sqrMagnitude > 0.0001f
                ? rayDirection.normalized
                : Vector2.up;

        Vector2 perpendicular =
            new Vector2(
                -direction.y,
                direction.x
            );

        Bounds bounds = box.bounds;

        float halfRayDepth =
            0.5f *
            (
                Mathf.Abs(direction.x) * bounds.size.x +
                Mathf.Abs(direction.y) * bounds.size.y
            );

        float halfRayWidth =
            0.5f *
            (
                Mathf.Abs(perpendicular.x) * bounds.size.x +
                Mathf.Abs(perpendicular.y) * bounds.size.y
            );

        float distance =
            halfRayDepth * 2f +
            rayPadding * 2f;

        for (int i = 0; i < rayCount; i++)
        {
            float t =
                rayCount == 1
                    ? 0.5f
                    : (float)i / (rayCount - 1);

            float offset =
                Mathf.Lerp(
                    -halfRayWidth,
                    halfRayWidth,
                    t
                );

            Vector2 origin =
                (Vector2)bounds.center
                - direction *
                (halfRayDepth + rayPadding)
                + perpendicular *
                offset;

            Gizmos.DrawRay(
                origin,
                direction * distance
            );
        }
    }

#endif
}