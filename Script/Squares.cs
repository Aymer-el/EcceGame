using UnityEngine;
using System.Collections;

/**
 * Each Piece has an attached Square:
 * This square is used to assess whether or coordinates of a place already
 * contains a piece.
 */
public class Square
{
    /**** Dependency ****/
    public Board Board { get; private set; }
    public Color Color { get; private set; }
    // set of x and y to retrieve squares in tables' board.
    public Coordinates Coordinates { get; private set; }
    public Vector3 position = new Vector3();

    /**** Game Mode ****/
    // Tells us whether or not a square is occupied.
    public bool isOccupied { get; private set; }
    public Square SetIsOccuped() {
        isOccupied = !isOccupied;
        return this;
    }

    /**
     * Constructor with a relative board and an unoccupied place
     * this will be change from the dependency of Piece, mostlikely through either
     * through Global or from Board.
     */
    public Square(Board board, Color colorType, int x, int y) {
        this.Board = board;
        this.Color = colorType;
        Coordinates = new Coordinates(x, y);
        this.position = Vector3.right * (-21 + (x * 2)) +
            Vector3.down * y * 2;
        isOccupied = false;
    }
}

public class Square_Entry : Square {
    public Square_Entry(Board board, Color color, int x, int y)
        : base(board, color, x, y) { }
};

public class Square_Delta : Square {
    public Square_Delta(Board board, Color color, int x, int y)
        : base(board, color, x, y) { }
};
