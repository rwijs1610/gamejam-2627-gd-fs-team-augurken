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
        originalColor = Image.color;
    }
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.dKey.isPressed)
        {
            Image.color = Color;
        }
        else
        {
            Image.color = originalColor;
        }
    }
}
