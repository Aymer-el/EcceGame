using UnityEngine;
using System.Collections;

public static class GameLogic
{
    public static bool NormalMove(Vector2 origin, Vector2 move)
  {
    return (move.x > 0 && (move.x >= origin.x - 2 && move.x <= origin.x + 2) &&
      (move.y >= origin.y - 2 && move.y <= origin.y + 2));
  }

}