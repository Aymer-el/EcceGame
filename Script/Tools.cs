using UnityEngine;
using System.Collections;


/**
 * Find the coordinates (x, y) on a two dimensional tables
 * with a table and an entry.
 */
public static class Tools<T> {
    public static Coordinates GetIndexOf(T [,] arr, Piece piece){
        for (int x = 0; x < arr.GetLength(0); ++x) {
            for (int y = 0; y < arr.GetLength(1); ++y) {
                if (ReferenceEquals(arr[x, y], piece)) return new Coordinates(x, y);
            }
        }
        return new Coordinates(-1, -1);
    }
}

/**
 * set of x and y. Mostly used to coordinates pieces and squares.
 */
public class Coordinates {
    public Coordinates(Vector3 vector){
        this.X = (int)vector.x;
        this.Y = (int)vector.y;
    }

    public Coordinates(int x, int y) {
        this.X = x;
        this.Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }

    override public string ToString() {
        return ("x is " + X.ToString() + "y is " + Y.ToString());
    }
}