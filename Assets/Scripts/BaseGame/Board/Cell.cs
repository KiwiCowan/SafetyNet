using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public event EventHandler Click;

    private TextMeshPro _text;

    public bool IsEnabled { get; set; } = true;

    public void SetText(string text)
    {
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshPro>();
        }

        _text.text = text;
    }

    private void OnMouseDown()
    {
        if (!IsEnabled)
        {
            return;
        }

        Click?.Invoke(this, EventArgs.Empty);
    }
}
