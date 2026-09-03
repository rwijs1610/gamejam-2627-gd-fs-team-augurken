using UnityEngine;
using UnityEngine.UI;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private Vector2 speed = new Vector2(0.05f, 0.03f);

    private Vector2 offset;

    private void Update()
    {
        offset += speed * Time.deltaTime;
        offset.x = Mathf.Repeat(offset.x, 1f);
        offset.y = Mathf.Repeat(offset.y, 1f);

        background.uvRect = new Rect(
            offset.x,
            offset.y,
            background.uvRect.width,
            background.uvRect.height
        );
    }
}