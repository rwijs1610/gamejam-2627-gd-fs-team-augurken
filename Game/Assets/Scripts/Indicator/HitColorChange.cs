
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HitColorChange : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerId = 1;

    [Header("References")]
    [SerializeField] private HitGameplay hitGameplay;

    [Header("Input")]
    [SerializeField]
    private InputActionReference[] laneActions =
        new InputActionReference[4];

    [Header("Hit Image")]
    [SerializeField]
    private GameObject hitImagePrefab;

    [Header("Lane X Positions")]
    [SerializeField]
    private float[] laneXPositions =
        new float[4];

    [Header("Position")]
    [SerializeField]
    private float endY = 3f;

    private Action<InputAction.CallbackContext>[] callbacks;

    private GameObject[] activeImages =
        new GameObject[4];

    private void Awake()
    {
        callbacks =
            new Action<InputAction.CallbackContext>[4];
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
                context => OnLaneInput(lane, context);

            laneActions[i].action.performed +=
                callbacks[i];

            laneActions[i].action.canceled +=
                callbacks[i];

            laneActions[i].action.Enable();
        }
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

                laneActions[i].action.canceled -=
                    callbacks[i];
            }

            laneActions[i].action.Disable();
        }
    }

    private void OnLaneInput(
        int lane,
        InputAction.CallbackContext context)
    {
        if (lane < 0 || lane >= 4)
            return;

        if (context.performed)
        {
            SpawnHitImage(lane);
        }
        else if (context.canceled)
        {
            RemoveHitImage(lane);
        }
    }

    private void SpawnHitImage(int lane)
    {
        if (hitImagePrefab == null)
        {
            Debug.LogError(
                "[HitColorChange] Hit Image Prefab is not assigned.",
                this
            );

            return;
        }

        if (laneXPositions == null ||
            laneXPositions.Length < 4)
        {
            Debug.LogError(
                "[HitColorChange] Need 4 lane X positions.",
                this
            );

            return;
        }

        // Remove existing image first
        RemoveHitImage(lane);

        Vector3 position =
            new Vector3(
                laneXPositions[lane],
                endY,
                0f
            );

        activeImages[lane] =
            Instantiate(
                hitImagePrefab,
                position,
                Quaternion.identity
            );

        // Make sure the UI image gets the correct color
        Image image =
            activeImages[lane].GetComponent<Image>();

        if (image == null)
        {
            image =
                activeImages[lane].GetComponentInChildren<Image>();
        }

        if (image != null)
        {
            // The prefab's own Image color is used.
            // You can set the desired color directly on the prefab.
        }
    }

    private void RemoveHitImage(int lane)
    {
        if (lane < 0 || lane >= activeImages.Length)
            return;

        if (activeImages[lane] != null)
        {
            Destroy(activeImages[lane]);
            activeImages[lane] = null;
        }
    }
}