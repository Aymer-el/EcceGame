using UnityEngine;

public class Global : MonoBehaviour
{

  /**** Dependency ****/
  // Board or awaiting pieces to enter in game or that.
  public GameObject whitePiecePrefab;
  public GameObject blackPiecePrefab;


  /**** Relative to Game Object and View ****/
  // Unique set of Pieces.
  public Piece[,] pieces = new Piece[8, 8];
  public Piece[,] piecesNotOnBoard = new Piece[2, 8];
  public int Player = 0;
  // Board du DamiersEcce.
  private Board_Ecce Board_Ecce;
  /**** Action ****/
  Vector2 mouseOver;
  Vector2 startDrag;
  bool HasMoved = true;

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
    this.GeneratePieces();
  }

  /*
   * Allow us to detect click, and drag and drop event.
   */
  private void Update()
  {
    this.UpdateMouseOver();
  }

  /*
   * Once the user ask to move a Piece
   */
  private void UpdateMouseOver()
  {
    if (Camera.main && Input.GetMouseButtonDown(0))
    {
      // Getting physics and intersection
      bool physicsBoardEcce = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out RaycastHit hit, 100.0f, LayerMask.GetMask("Board_Ecce"));
      if (physicsBoardEcce)
      {
        // saving where the move has begun
        mouseOver.x = (int)hit.point.x;
        mouseOver.y = (int)hit.point.z;
        if (Input.GetMouseButtonDown(0))
        {
          if(selectedPiece == null)
          {
            // Select one piece
            TrySelectPiece(mouseOver);
          }
          else if(GetPiece(mouseOver) == null)
          {
            // Verify There are no piece in the next move
            TryMovePiece(selectedPiece, mouseOver, startDrag);
          } else
          {
            // Either change piece movement or eat
            Debug.Log("in reselect");
            TrySelectPiece(mouseOver);
          }
        }
      }
      bool physicsWhiteBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out _, 100.0f, LayerMask.GetMask("WhiteBanch"));
      if (physicsWhiteBanch)
      {
        selectedPiece = GetPieceOfBanch(Player);
      }

      bool physicsBlackBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out _, 100.0f, LayerMask.GetMask("BlackBanch"));
      if(physicsBlackBanch)
      {
        selectedPiece = GetPieceOfBanch(Player);
        startDrag = new Vector2(-2 * caseLength + 1, 3 * caseLength);
      }
    }
  }

  private Piece GetPiece(Vector2 position)
  {
    // If Out of bounds
    if (position.x < 0 || position.x >= pieces.Length * caseLength
      && position.y < 0 || position.y >= pieces.Length * caseLength)
      return null;

    // Getting the pieces out of the array
    return pieces[
      (int)ToArrayCoordinates(position).x,
      (int)ToArrayCoordinates(position).y
      ];
  }

  private void TrySelectPiece(Vector2 mouseOver)
  {
    selectedPiece = GetPiece(mouseOver);
    startDrag = mouseOver;
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
    p.transform.position =
      (Vector3.right * ToBoardCoordinates(mouseOver).x) +
      (Vector3.forward * ToBoardCoordinates(mouseOver).y) +
      (Vector3.up * 1);
    // Checking so that unactive are not disturbing movement.
    if (startDrag.x > 0 && startDrag.y > 0)
    pieces[
      (int)ToArrayCoordinates(startDrag).x,
      (int)ToArrayCoordinates(startDrag).y
      ] = null;
    pieces[
      (int)ToArrayCoordinates(mouseOver).x,
      (int)ToArrayCoordinates(mouseOver).y
      ] = p;
    FinishTurn();
  }

  /*
   * Set of board of Entries Generator for both types.
   */
  private void GeneratePieces()
  {
    /*
    pieces[1, 1] = GeneratePiece(whitePiecePrefab,
      new Vector2(1 * caseLength + 1, 1 * caseLength + 1));
    pieces[6, 1] = GeneratePiece(blackPiecePrefab,
      new Vector2(6 * caseLength + 1, 1 * caseLength + 1));
    */
    for (var i = 0; i < 2; i++)
    {
      for (var j = 0; j < 8; j++)
      {
        Piece piece;
        if(i % 2 == 0)
        {
          piece = GeneratePiece(whitePiecePrefab,
      ToBoardCoordinates(new Vector2(-2 * caseLength + 1, 3 * caseLength)));
        } else
        {
          piece = GeneratePiece(blackPiecePrefab,
      ToBoardCoordinates(new Vector2(-2 * caseLength + 1, 5 * caseLength)));
        }
        piecesNotOnBoard[i, j] = piece;
        startDrag = ToBoardCoordinates(new Vector2(-2 * caseLength + 1, 5 * caseLength));
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

  private Piece GetPieceOfBanch(int Player)
  {
    var i = 0;
    var found = false;
    while (i < 7 && !found)
    {
      if (piecesNotOnBoard[Player, i] != null)
      {
        found = true;
      }
      else
      {
        i++;
      }
    }
    Piece piece = piecesNotOnBoard[Player, i];
    piecesNotOnBoard[Player, i] = null;
    return piece;
  }

  public void FinishTurn()
  {
    if(Player == 0)
    {
      Player = 1;
    } else
    {
      Player = 0;
    }
    startDrag = mouseOver;
    mouseOver = new Vector2();
    selectedPiece = null;
  }

  /** When First Piece is moved, regenerate one **/

}