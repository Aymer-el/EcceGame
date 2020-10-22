using UnityEngine;
using System.Collections;

public static class GameLogic
{
    public static bool IsMovePossible(bool CanGoDiagonal, Vector2 origin, Vector2 move) {
    if(!CanGoDiagonal)
    {
      // Standard piece move
      return (
        ((move.x +2 == origin.x || move.x -2 == origin.x) && move.y == origin.y)
        ||
        ((move.y + 2 == origin.y || move.y - 2 == origin.y) && move.x == origin.x)
        );
    } else
    {
      // Ecce piece move
      return (
      move.x > 0 && (move.x >= origin.x - 2 && move.x <= origin.x + 2) &&
      move.y > 0 && (move.y >= origin.y - 2 && move.y <= origin.y + 2) &&
      // The piece is selected a second time
      (move.x != origin.x || move.y != origin.y));
    }
  }

}