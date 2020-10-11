using UnityEngine;
using System.Collections;

/*
 * Class for pieces ready to be placed on Ecce's board, GameObject: _boardOfInCal.
 */
public class Board_IO : Board {
    
    public Color Color { get; private set; }

    public Board_IO Build_Board_IO(Color color) {
        Color = color;
        Squares = new Square[1, 2];
        this.GenerateSquares();
        if(color.ColorType == ColorEnum.White) {
            // BoardPosition = Vector3.up * 1 + Vector3.forward * 2.5f + Vector3.left * 12;
        } else {
            // BoardPosition = Vector3.up * 1 + Vector3.forward * 2.5f + Vector3.left * 0;
        }
        return this;
    }

    /*
     * Set of Squares Generator.
     */
    protected override Square[,] GenerateSquares()
    {
        bool switchColor = false;
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                Squares[i, j] = j % 2 == 0 && switchColor ?
                    new Square(this, new Color(ColorEnum.White), i, j)
                    : new Square(this, new Color(ColorEnum.Black), i, j);
                switchColor = !switchColor;
            }
        }
        return this.Squares;
    }

    public Board_IO SetColor(ColorEnum color)
    {
        this.Color = new Color(color);
        return this;
    }
}
