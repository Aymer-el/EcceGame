using UnityEngine;
using UnityEngine.UI;

public class Global : MonoBehaviour
{

  /**** Dependency ****/
  // Board or awaiting pieces to enter in game or that.
  public GameObject whitePiecePrefab;
  public GameObject blackPiecePrefab;
  public Text scoreText;
  public Text piecesLeftBlack;
  public Text piecesLeftWhite;

  /**** Relative to Game Object and View ****/
  // Unique set of Pieces.
  public Piece[,] pieces = new Piece[8, 8];
  public Piece[,] newPiecesNotOnBoard = new Piece[2, 8];
  public int player = 0;
  // Board du DamiersEcce.
  private Board_Ecce Board_Ecce;
  /**** Action ****/
  Vector2 mouseOver;
  Vector2 startDrag;


  int scoreWhite = 0;
  int scoreBlack = 0;

  private readonly int caseLength = 2;

  /**** View ****/
  private Piece selectedPiece;

  /*
   * Gather all components.
   * Generate Pieces.
   */
  public void Awake()
  {
    Board_Ecce = GetComponentInChildren<Board_Ecce>();
    scoreText = GetComponentInChildren<Text>();
    piecesLeftBlack = GameObject.Find("piecesLeftBlack").GetComponent<Text>();
    piecesLeftWhite = GameObject.Find("piecesLeftWhite").GetComponent<Text>();
    this.GeneratePieces();
  }

  /*
   * Allow us to detect click, and drag and drop event.
   */
  private void Update()
  {
    this.UpdateMouseOver();
    scoreText.text =
      "White score: " + scoreWhite.ToString() +
      "   -- nPal GAME --   " +
    "Black score: " + scoreBlack.ToString();
    piecesLeftWhite.text = "White pieces \n to play:" + NumberOfANewPiece(0).ToString();
    piecesLeftBlack.text = "Black pieces \n to play:" + NumberOfANewPiece(1).ToString();
  }

  /*
   * Once the user ask to move a Piece
   */
  private void UpdateMouseOver()
  {
    if (Camera.main && Input.GetMouseButtonDown(0))
    {
      bool physicsBoardEcce = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out RaycastHit hit, 25.0f, LayerMask.GetMask("Board_Ecce"));
      bool physicsBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit1, 50f, LayerMask.GetMask("Banch"));
      if (physicsBoardEcce || physicsBanch)
      {
        // Saving mouseOver
        mouseOver.x = (int)hit.point.x;
        mouseOver.y = (int)hit.point.z;
        if (Input.GetMouseButtonDown(0))
        {
          if (selectedPiece == null)
          {
            // Selecting a new piece
            if (physicsBanch)
            {
              TryPlaceNewPiece(player);
            } else
            // Selecting a piece
            {
              TrySelectPiece(mouseOver, player);
            }
          }
          else
          {
            Piece advPiece = GetPiece(mouseOver);
            if (advPiece != null && !advPiece.name.Contains(this.selectedPiece.name.Substring(0, 5)))
            {
              // Eating piece
              if (GameLogic.IsMovePossible(true, selectedPiece.isEcce,
                ToBoardCoordinates(startDrag), ToBoardCoordinates(mouseOver)))
              {
                RemovingPiece(ToArrayCoordinates(mouseOver));
                TryMovePiece(selectedPiece, mouseOver, startDrag);
              }
            }
            else
            {
              // Moving piece
              // If there is no piece on the case && if it is a possible move
              if (GetPiece(mouseOver) == null && GameLogic.IsMovePossible(selectedPiece.isEcce, true,
                ToBoardCoordinates(startDrag), ToBoardCoordinates(mouseOver)))
              {
                TryMovePiece(selectedPiece, mouseOver, startDrag);
              }
              // Selecting another piece
              else
              {
                DeselectPiece(selectedPiece, startDrag);
                TrySelectPiece(mouseOver, player);
              }
            }
          }
        }
      }
    }
  }

  private Piece GetPiece(Vector2 position)
  {
    // Getting the piece out of the array
    return pieces[
      (int)ToArrayCoordinates(position).x,
      (int)ToArrayCoordinates(position).y
      ];
  }

  private void TrySelectPiece(Vector2 mouseOver, int player)
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
    }
  }

  public Vector2 ToArrayCoordinates(Vector2 c)
  {
    return new Vector2(
      Mathf.FloorToInt(c.x / caseLength),
      Mathf.FloorToInt(c.y / caseLength)
   );
  }

  public Vector2 ToBoardCoordinates(Vector2 c)
  {
    return new Vector2(
      Mathf.FloorToInt(c.x / caseLength) * caseLength + caseLength / 2,
      Mathf.FloorToInt(c.y / caseLength) * caseLength + caseLength / 2
    );
  }

  public void TryMovePiece(Piece p, Vector2 mouseOver, Vector2 startDrag)
  {
    // Moving piece
    p.transform.position =
      (Vector3.right * ToBoardCoordinates(mouseOver).x) +
      (Vector3.forward * ToBoardCoordinates(mouseOver).y) +
      (Vector3.up * 1);
    // Deleting piece in array
    pieces[
      (int)ToArrayCoordinates(startDrag).x,
      (int)ToArrayCoordinates(startDrag).y
      ] = null;
    // Placing piece in array
    pieces[
      (int)ToArrayCoordinates(mouseOver).x,
      (int)ToArrayCoordinates(mouseOver).y
      ] = p;
    // In case of a first piece move
    //TryPlaceNewPiece(player, ToArrayCoordinates(startDrag));
    CheckPieceEvolution(p, ToArrayCoordinates(mouseOver));
    CheckOneUp(p, ToArrayCoordinates(mouseOver));
    FinishTurn();
    DeselectPiece(selectedPiece, mouseOver);
  }


  /*
   * Set of board of Entries Generator for both types.
   */
  private void GeneratePieces()
  {
    for (var i = 0; i < 2; i++)
    {
      for (var j = 0; j < 7; j++)
      {
        Piece piece;
        if (i % 2 == 0)
        {
          piece = GeneratePiece(whitePiecePrefab,
            ToBoardCoordinates(new Vector2(-3, 2)));
        } else
        {
          piece = GeneratePiece(blackPiecePrefab,
            ToBoardCoordinates(new Vector2(18, 2)));
        }
        newPiecesNotOnBoard[i, j] = piece;
        startDrag = ToBoardCoordinates(new Vector2(-2 * caseLength + 1, 5 * caseLength));
        // Moving piece
      }
    }
  }

  /**
  * Single Piece Generator.
  */
  private Piece GeneratePiece(GameObject piecePrefab, Vector2 coordinate)
  {
    GameObject go = Instantiate(piecePrefab) as GameObject;
    go.AddComponent<Piece>();
    go.transform.SetParent(Board_Ecce.transform);
    Piece piece = go.GetComponent<Piece>();
    piece.transform.position =
      (Vector3.right * coordinate.x) +
      (Vector3.forward * coordinate.y) +
      (Vector3.up * 1);
    return piece;
  }

  public void RemovingPiece(Vector2 mouseOver)
  {
    pieces[(int)mouseOver.x, (int)mouseOver.y].gameObject.SetActive(false);
    pieces[(int)mouseOver.x, (int)mouseOver.y] = null;
  }

  public void FinishTurn()
  {
    if (player == 0)
    {
      player = 1;
    } else
    {
      player = 0;
    }
    startDrag = mouseOver;
    mouseOver = new Vector2();
  }

  public void DeselectPiece(Piece piece, Vector2 startDrag)
  {
    // Deselect the piece
    selectedPiece.GetComponent<MeshRenderer>().material = selectedPiece.myMaterials[0];
    selectedPiece.transform.position =
      (Vector3.right * ToBoardCoordinates(startDrag).x) +
      (Vector3.forward * ToBoardCoordinates(startDrag).y) +
      (Vector3.up * 1);
    selectedPiece = null;
  }

  public void TryPlaceNewPiece(int player)
  {
    if (player == 0 && pieces[1, 1] == null)
    {
      selectedPiece = GetANewPiece(player);
      TryMovePiece(selectedPiece,
        new Vector2(1 * caseLength, 1 * caseLength),
        new Vector2(1 * caseLength, 1 * caseLength));
    }
    if (player == 1 && pieces[6, 1] == null)
    {
      selectedPiece = GetANewPiece(player);
      TryMovePiece(selectedPiece,
        new Vector2(6 * caseLength, 1 * caseLength),
        new Vector2(6 * caseLength, 1 * caseLength));
    }
  }

  private Piece GetANewPiece(int player)
  {
    var i = 0;
    var found = false;
    while (i <= 7 && !found)
    {
      if (newPiecesNotOnBoard[player, i] != null)
      {
        found = true;
      }
      else
      {
        i++;
      }
    }
    Piece piece = newPiecesNotOnBoard[player, i];
    newPiecesNotOnBoard[player, i] = null;
    return piece;
  }

  public Piece CheckPieceEvolution(Piece p, Vector2 mouseOver)
  {
    if(mouseOver.x == 1 && mouseOver.y == 6 && p.name.Contains("white")
      ||
       mouseOver.x == 6 && mouseOver.y == 6 && p.name.Contains("black")
      )
    {
      p.isEcce = true;
    }
    return p;
  }

  public void CheckOneUp(Piece p, Vector2 mouseOver)
  {
    if (mouseOver.x == 1 && mouseOver.y == 1 && p.name.Contains("black")
      ||
       mouseOver.x == 6 && mouseOver.y == 1 && p.name.Contains("white")
      )
    {
      if(player == 0)
      {
        scoreWhite++;
      } else
      {
        scoreBlack++;
      }
      RemovingPiece(mouseOver);
    }
  }

  public bool IsPlayerPickingRightColorPiece(Piece piece, int player)
  {
    return piece.name.Contains("white") && player == 0
      ||
           piece.name.Contains("black") && player == 1;
  }

  public int NumberOfANewPiece(int player)
  {
    var i = 0;
    var found = false;
    while (i < 7 && !found)
    {
      if (newPiecesNotOnBoard[player, i] != null)
      {
        found = true;
      }
      else
      {
        i++;
      }
    }
    return newPiecesNotOnBoard.Length/2 - (i +1);
  }

  /** When First Piece is moved, regenerate one **/

}