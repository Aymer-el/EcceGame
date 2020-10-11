using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Each Piece represents an instanciated game object, either _white or _black
 */ 
public class Piece: MonoBehaviour {

    /**** Dependency ****/
    public Board Board { get; private set; }
    public Square Square { get; private set; }
    public Color Color { get; private set; }
    // Allows player to move its pieces or/and to offset pieces from the adversaries
    public bool isEcce;

    /**** View ****/
   
    // Space between spaces
    protected int pieceMargin = 2;
    // Space to Center Pieces
    protected Vector3 piecePadding = new Vector3(0, 0, 0);

    /*
     * Set piece to the state relative to its move, hence to Board and Square.
     */
    public Piece SetPiece(Color color) {
        this.Color = color;
        this.isEcce = false;
        this.transform.position = this.transform.position + Vector3.down * -1;
        return this;
    }

    /**
     * Function that find the ratio between Boards and views coordinates and positions.
     */
    public Piece PlacePiece() {
        Vector3 pieceCoordinatePosition = Vector3.back * Square.Coordinates.X * pieceMargin;
        pieceCoordinatePosition += Vector3.right * (Square.Coordinates.Y * pieceMargin + piecePadding.x);
        pieceCoordinatePosition += Vector3.down * -1;
        this.transform.position = pieceCoordinatePosition;
        return this;
    }
}