using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerLaneInput : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private int playerId = 1;

    [Header("References")]
    [SerializeField]
    private HitGameplay hitGameplay;

    [SerializeField]
    private PlayerHitbox[] laneHitboxes =
        new PlayerHitbox[4];

    [SerializeField]
    private InputActionReference[] laneActions =
        new InputActionReference[4];

    private Action<InputAction.CallbackContext>[] callbacks;

    public int PlayerId => playerId;

    private void Awake()
    {
        callbacks =
            new Action<InputAction.CallbackContext>[4];

        // Safety checks
        if (laneHitboxes == null)
        {
            Debug.LogError(
                $"[PlayerLaneInput] LaneHitboxes array is null | " +
                $"Player={playerId}"
            );

            return;
        }

        if (laneActions == null)
        {
            Debug.LogError(
                $"[PlayerLaneInput] LaneActions array is null | " +
                $"Player={playerId}"
            );

            return;
        }

        if (laneHitboxes.Length != 4)
        {
            Debug.LogWarning(
                $"[PlayerLaneInput] Expected 4 hitboxes, " +
                $"but found {laneHitboxes.Length} | " +
                $"Player={playerId}"
            );
        }

        if (laneActions.Length != 4)
        {
            Debug.LogWarning(
                $"[PlayerLaneInput] Expected 4 input actions, " +
                $"but found {laneActions.Length} | " +
                $"Player={playerId}"
            );
        }
    }

    private void OnEnable()
    {
        if (laneActions == null)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (i >= laneActions.Length)
                continue;

            if (laneActions[i] == null)
                continue;

            int lane = i;

            callbacks[i] =
                _ => PressLane(lane);

            laneActions[i].action.performed +=
                callbacks[i];

            laneActions[i].action.Enable();
        }

        Debug.Log(
            $"[PlayerLaneInput] Enabled | " +
            $"Player={playerId}"
        );
    }

    private void OnDisable()
    {
        if (laneActions == null)
            return;

        for (int i = 0; i < 4; i++)
        {
            if (i >= laneActions.Length)
                continue;

            if (laneActions[i] == null)
                continue;

            if (callbacks[i] != null)
            {
                laneActions[i].action.performed -=
                    callbacks[i];
            }

            laneActions[i].action.Disable();
        }
    }

    public int GetLane(PlayerHitbox hitbox)
    {
        if (hitbox == null)
            return -1;

        if (laneHitboxes == null)
            return -1;

        for (int i = 0; i < laneHitboxes.Length; i++)
        {
            if (laneHitboxes[i] == hitbox)
                return i;
        }

        return -1;
    }

    private void PressLane(int lane)
    {
        Debug.Log(
            $"[PlayerLaneInput] INPUT | " +
            $"Player={playerId} | Lane={lane}"
        );

        if (hitGameplay == null)
        {
            Debug.LogError(
                "[PlayerLaneInput] HitGameplay is not assigned."
            );

            return;
        }

        if (laneHitboxes == null)
        {
            Debug.LogError(
                "[PlayerLaneInput] LaneHitboxes array is null."
            );

            return;
        }

        if (lane < 0 || lane >= laneHitboxes.Length)
        {
            Debug.LogError(
                $"[PlayerLaneInput] Invalid lane {lane}."
            );

            return;
        }

        PlayerHitbox hitbox =
            laneHitboxes[lane];

        if (hitbox == null)
        {
            Debug.LogError(
                $"[PlayerLaneInput] Lane {lane} " +
                $"has no PlayerHitbox assigned."
            );

            return;
        }

        // Pass player and lane directly.
        // No hierarchy lookup is needed.
        hitGameplay.TryHit(
            hitbox,
            playerId,
            lane
        );
    }
}