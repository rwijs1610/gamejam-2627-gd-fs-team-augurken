using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreImage : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreSprites;
    [SerializeField] private GameObject instantiate;
    [SerializeField] private GameObject endMovement;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private float randomOffset = 0.35f;
    [SerializeField] private float holdDuration = 0.5f;
    [SerializeField] private InputActionReference Leftleft;

    private void Update()
    {

    }

    private void OnEnable()
    {
        Leftleft.action.started += leftleft;
    }

    private void OnDisable()
    {
        Leftleft.action.started -= leftleft;
    }

    private void leftleft(InputAction.CallbackContext obj)
    {
        if (scoreSprites == null || scoreSprites.Length == 0)
        {
            Debug.Log("je hebt geen sprites ezel"); return;
        }
            

        var prefab = scoreSprites[UnityEngine.Random.Range(0, scoreSprites.Length)];
        if (prefab == null)
            return;

        var go = Instantiate(prefab, instantiate.transform.position, Quaternion.identity);
        var mover = go.AddComponent<ScoreMover>();
        mover.Init(endMovement != null ? endMovement.transform.position : instantiate.transform.position, moveDuration, randomOffset, holdDuration);
    }
}

