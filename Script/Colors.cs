using UnityEngine;
using System.Collections;

/**
 * Allows us to detect the color of a piece or special square.
 */

public enum ColorEnum { White, Black };

public class Color
{
    public ColorEnum ColorType { get; set; }

    public Color(ColorEnum color) {
        this.ColorType = color;
    }
}