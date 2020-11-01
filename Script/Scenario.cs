using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : Global
{

  public new void Awake()
  {
    scoreWhite = GameObject.Find("scoreWhite").GetComponent<Text>();
    scoreBlack = GameObject.Find("scoreBlack").GetComponent<Text>();
    scoreBlack.gameObject.SetActive(false);
    scoreWhite.gameObject.SetActive(false);
    GameObject.Find("score").SetActive(false);
    this.GeneratePieces();
    this.InitialiseScenario();
  }


  void InitialiseScenario ()
  {
    TryPlaceNewPiece(0);
    TryPlaceNewPiece(1);
    TrySelectPiece(new Vector2(2, 2), 0);
    TryMovePiece(selectedPiece,
        new Vector2(5 * caseLength, 1 * caseLength),
        new Vector2(1 * caseLength, 1 * caseLength));
  }
}
