using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class NoteHitbox : MonoBehaviour
{
    public const string RequiredTag = "NoteHitbox";

    [Header("Identity")]
    [field: SerializeField]
    public int PlayerId { get; private set; }

    [field: SerializeField]
    public int LaneId { get; private set; }

    public bool IsConsumed { get; private set; }

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        if (!CompareTag(RequiredTag))
        {
            Debug.LogWarning(
                $"[NoteHitbox] {name} is missing the " +
                $"'{RequiredTag}' tag.",
                this
            );
        }

        Debug.Log(
            $"[NoteHitbox] Created | " +
            $"Name={name} | " +
            $"Player={PlayerId} | " +
            $"Lane={LaneId} | " +
            $"Position={transform.position}"
        );
    }

    public void Consume()
    {
        if (IsConsumed)
            return;

        IsConsumed = true;

        Debug.Log(
            $"[NoteHitbox] HIT -> Destroying '{name}'"
        );

        Destroy(gameObject);
    }
}