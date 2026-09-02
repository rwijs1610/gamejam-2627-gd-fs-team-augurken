using System;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerLaneInput : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private int playerId;

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

    private void Awake()
    {
        callbacks =
            new Action<InputAction.CallbackContext>[4];
    }

    private void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
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
            $"[PlayerLaneInput] Enabled | Player={playerId}"
        );
    }

    private void OnDisable()
    {
        for (int i = 0; i < 4; i++)
        {
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

        if (laneHitboxes[lane] == null)
        {
            Debug.LogError(
                $"[PlayerLaneInput] Lane {lane} " +
                $"has no PlayerHitbox assigned."
            );

            return;
        }

        PlayerHitbox hitbox =
            laneHitboxes[lane];

        if (hitbox.PlayerId != playerId)
        {
            Debug.LogError(
                $"[PlayerLaneInput] Player mismatch! " +
                $"Input Player={playerId}, " +
                $"Hitbox Player={hitbox.PlayerId}"
            );

            return;
        }

        hitGameplay.TryHit(hitbox);
    }
}