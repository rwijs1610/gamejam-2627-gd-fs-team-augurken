using UnityEngine;

public class TextRotation : MonoBehaviour
{
    [SerializeField] private float Turnspeed = 200f;
    [SerializeField] private float maxAngle = 45f;

    void Update()
    {
        float angle = Mathf.Sin(Time.time * Turnspeed) * maxAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
