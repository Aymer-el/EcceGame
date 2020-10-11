using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class du Damiers Ecce, in which is the GameObject _boardEcce.
 */
public class Board_Ecce : Board {
    
    private int forwardConver = 0;
    private float rightConvert = 0;

    public Board_Ecce() {
            // In reference to Board position, cf: Unity's Transform
        BoardPosition = Vector3.forward * forwardConver + Vector3.right * rightConvert;
    }

    public Vector3 BoardToViewVector3(Vector3 vectorOfView)
    {
        Vector3 boardVector = new Vector3();
        boardVector.Set(vectorOfView.x, vectorOfView.y, 0);
        boardVector = boardVector / 2;
        boardVector.Set(Mathf.Abs(Mathf.FloorToInt(boardVector.y)),
                        Mathf.Abs(Mathf.FloorToInt(boardVector.x)), -1);
        return boardVector;
    }

    public void Start() {
        Squares = new Square[numberOfSquares, numberOfSquares];
        this.GenerateSquares();
    }

    /**
     * Generator for a set of squares.
     */
    protected override Square[,] GenerateSquares() {
        for (int i = 0; i < 8; i++) {
            int countSquare = 0;
            for (int j = 0; j < 8; j++) {
                if ((i != 1 && (j != 6 || j != 1)) || (i != 6 && (j != 1 || j != 6))) {
                    Squares[i, j] = countSquare % 2 == 0 ?
                        new Square(this, new Color(ColorEnum.White), i, j)
                        : new Square(this, new Color(ColorEnum.Black), i, j);
                } else {
                    Squares[1, 1] = new Square_Entry(this, new Color(ColorEnum.Black), 1, 1);
                    Squares[1, 6] = new Square_Delta(this, new Color(ColorEnum.White), 1, 6);
                    Squares[6, 1] = new Square_Entry(this, new Color(ColorEnum.White), 1, 1);
                    Squares[6, 6] = new Square_Delta(this, new Color(ColorEnum.Black), 1, 1);
                }
                countSquare++;
            }
        }
        return this.Squares;
    }
}
