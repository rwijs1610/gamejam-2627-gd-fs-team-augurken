using UnityEngine;

public class NormalBurger : MonoBehaviour
{
    [SerializeField] float downForce = 0.01f;

    void OnEnable()
    {
        FallingNotes.OnBPMChanged += ChangeSpeed;
    }

    void OnDisable()
    {
        FallingNotes.OnBPMChanged -= ChangeSpeed;
    }

    void Start()
    {
    }

    void Update()
    {
        gameObject.transform.Translate(
            0f,
            downForce * Time.deltaTime,
            0f
        );
    }

    void ChangeSpeed(float bpm)
    {
        downForce = bpm / 60f;
    }
}