using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCube : MonoBehaviour
{
}

public class Scenario : Global
{
  public GameObject tutorialCubePrefab;
  public TutorialCube[] tutorialCubes = new TutorialCube[9];

  public new void Awake()
  {
    scoreWhite = GameObject.Find("scoreWhite").GetComponent<Text>();
    scoreBlack = GameObject.Find("scoreBlack").GetComponent<Text>();
    scoreBlack.gameObject.SetActive(false);
    scoreWhite.gameObject.SetActive(false);
    GameObject.Find("score").SetActive(false);
    this.GeneratePieces();
    for (var i = 0; i < tutorialCubes.Length; i++)
    {
      tutorialCubes[i] = GenerateCube(tutorialCubePrefab);
    }
    this.InitialiseScenario();
  }

  /**
* Single Piece Generator.
*/
  private TutorialCube GenerateCube(GameObject cubePrefab)
  {
    GameObject go = Instantiate(cubePrefab) as GameObject;
    go.AddComponent<TutorialCube>();
    TutorialCube tutorialCube = go.GetComponent<TutorialCube>();
    return tutorialCube;
  }

  private void PlaceCube(Piece piece, Vector2 piecePosition)
  {

    int count = 0;
    for (var i = -2; i < 4; i+=2)
    {
      for (var j = -2; j < 4; j+=2)
      {
        Vector2 boardCoordinate = new Vector2((i + piecePosition.x) + 1, (j + piecePosition.y) + 1);
        if (GetPiece(boardCoordinate) == null &&
          GameLogic.IsMovePossible(piece.isEcce, true, ToBoardCoordinates(piecePosition), ToBoardCoordinates(boardCoordinate)))
        {
          tutorialCubes[count].transform.position =
            (Vector3.right * boardCoordinate.x) +
            (Vector3.forward * boardCoordinate.y) +
            (Vector3.up * -0.5f);
          count++;
        }
      }
    }
  }

  void InitialiseScenario ()
  {
    TryPlaceNewPiece(0);
    TryPlaceNewPiece(1);
    TrySelectPiece(new Vector2(2, 2), 0);
    TryMovePiece(selectedPiece,
        new Vector2(5 * caseLength, 1 * caseLength),
        new Vector2(1 * caseLength, 1 * caseLength));
    TrySelectPiece(new Vector2(12, 2), 1);
  }

  protected new void TrySelectPiece(Vector2 mouseOver, int player)
  {
    Piece piece = GetPiece(mouseOver);
    if (piece != null && IsPlayerPickingRightColorPiece(piece, player))
    {
      selectedPiece = piece;
      startDrag = mouseOver;
      // Showing selection
      if (selectedPiece != null)
      {
        // Material selection
        selectedPiece.GetComponent<MeshRenderer>().material = selectedPiece.myMaterials[1];
        // Perspective selection
        selectedPiece.gameObject.transform.position =
          (Vector3.right * ToBoardCoordinates(mouseOver).x) +
          (Vector3.forward * ToBoardCoordinates(mouseOver).y) +
          (Vector3.up * 1.1f);
      }
      PlaceCube(piece, new Vector2(12,2));
    }
  }
}
