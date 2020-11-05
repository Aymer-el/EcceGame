using UnityEngine;
using System.Collections;

public static class GameLogic
{
    public static bool IsMovePossible(bool CanGoDiagonal, bool CanGoHorizontal, Vector2 origin, Vector2 move) {
    if(move.x >=0 && origin.x >= 0 && move.y >= 0 && move.y >= 0
      && move.x <= 8*2 && origin.x <= 8*2 && move.y <= 8*2 && move.y <= 8*2) {
      if (!CanGoDiagonal)
      {
        // Standard piece move
        return (
          ((move.x + 2 == origin.x || move.x - 2 == origin.x) && move.y == origin.y)
          ||
          ((move.y + 2 == origin.y || move.y - 2 == origin.y) && move.x == origin.x)
          );
      } else if (!CanGoHorizontal)
      {
        // Standard piece eating move
        return (
          ((move.x + 2 == origin.x || move.x - 2 == origin.x) && (move.y + 2 == origin.y || move.y - 2 == origin.y))
          ||
          ((move.y + 2 == origin.y || move.y - 2 == origin.y) && (move.x + 2 == origin.x || move.x - 2 == origin.x))
          );
      } else
      {
        // Ecce piece move or Ecce piece eating move
        return (
        move.x > 0 && (move.x >= origin.x - 2 && move.x <= origin.x + 2) &&
        move.y > 0 && (move.y >= origin.y - 2 && move.y <= origin.y + 2) &&
        // The piece is selected a second time
        (move.x != origin.x || move.y != origin.y));
      }
    } else
    {
      return false;
    }
  }
}