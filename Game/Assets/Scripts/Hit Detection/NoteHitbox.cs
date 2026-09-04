using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public sealed class NoteHitbox : MonoBehaviour
{
    public const string RequiredTag = "NoteHitbox";

    public bool IsConsumed { get; private set; }

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        if (_collider == null)
        {
            Debug.LogError(
                $"[NoteHitbox] Collider2D missing on '{name}'.",
                this
            );
        }

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