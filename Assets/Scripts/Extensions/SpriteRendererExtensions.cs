using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteRendererExtensions
{
    public static void SetAlphaToZero(this SpriteRenderer spriteRenderer)
    {
        var color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
    }
}
