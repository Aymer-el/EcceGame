using UnityEngine;
using System.Collections;

public abstract class Board: MonoBehaviour {
    /**** Dependency ****/

    // GameObject Board: (_boardEcce, _banchOfInCall, _banchOfOff, _banchOfPlaced)
    public GameObject _board;
    public Square[,] Squares { get; protected set; }

    /**** View ****/

    // Allows a correct piece placement
    public Vector3 BoardPosition { get; protected set; }
    // Number of pieces 
    protected int numberOfSquares = 8;

    /**
     * generators of for a set of squares.
     */
    protected abstract Square[,] GenerateSquares();

    /**
     * Dispatch a piece from one board to another, as for square.
     * As it does place it correctly on the view.
     */
    public Piece MovePiece(Piece piece, Coordinates destination) {
    return piece;//SetPiece(this, Squares[destination.X, destination.Y], piece.Color)
                    //.PlacePiece();
    }

}
