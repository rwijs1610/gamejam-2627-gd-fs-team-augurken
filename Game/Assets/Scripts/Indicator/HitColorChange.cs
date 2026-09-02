using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
public class HitColorChange : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private Color Color;

    private Color originalColor;

    private void Start()
    {
        if (Image == null)
        {
            Debug.LogWarning("HitColorChange: Image reference is not assigned.", this);
            return;
        }

        originalColor = Image.color;
    }
    private void Update()
    {
        if (Image == null)
        {
            return;
        }

        bool isPressed;
        if (Keyboard.current != null)
        {
            isPressed = Keyboard.current.dKey.isPressed;
        }
        else
        {
            isPressed = Input.GetKey(KeyCode.D);
        }

        if (isPressed)
        {
            float alpha = Mathf.Approximately(Color.a, 0f) ? originalColor.a : Color.a;
            Image.color = new UnityEngine.Color(Color.r, Color.g, Color.b, alpha);
        }
        else
        {
            Image.color = originalColor;
        }
    }
}
