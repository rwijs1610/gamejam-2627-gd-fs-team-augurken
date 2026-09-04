using UnityEngine;

public class Burgeruppies : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 2f;
    private Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float Y = startPos.y + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(transform.position.x, Y, transform.position.z);
    }
}
