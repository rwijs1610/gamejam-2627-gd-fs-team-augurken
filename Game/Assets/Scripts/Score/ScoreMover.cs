using UnityEngine;

public class ScoreMover : MonoBehaviour
{
    private Vector3 start;
    private Vector3 target;
    private float duration = 0.8f;
    private float elapsed;
    private float holdDuration = 0.5f;
    private int phase = 0; // 0 = image boven bewegen, 1 = image vasthouden, 2 = image naar beneden bewegen


    public void Init(Vector3 targetPos, float durationSeconds, float offset, float holdSeconds = 0.5f)
    {
        start = transform.position;
        var random = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0f);
        target = targetPos + random;
        duration = Mathf.Max(0.01f, durationSeconds);
        holdDuration = Mathf.Max(0f, holdSeconds);
        elapsed = 0f;
        phase = 0;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if (phase == 0)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(start, target, s);

            if (t >= 1f)
            {
                phase = 1;
                elapsed = 0f;
            }
        }
        else if (phase == 1)
        {
            if (elapsed >= holdDuration)
            {
                phase = 2;
                elapsed = 0f;
            }
        }
        else
        {
            float t = Mathf.Clamp01(elapsed / duration);
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(target, start, s);

            if (t >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
