using UnityEngine;

public class Global : MonoBehaviour
{
  /**** Relative to Game Object and View ****/
  // Unique set of Pieces.
  public Piece[,] pieces = new Piece[8, 8];

  /**** Dependency ****/
  // Board or awaiting pieces to enter in game or that.
  public GameObject whitePiecePrefab;
  public GameObject blackPiecePrefab;
  // Board du DamiersEcce.
  private Board_Ecce Board_Ecce;
  private Board_Ecce Board_EcceLayerCastDetection;
  /**** Action ****/
  Vector2 mouseOver;
  Vector2 startDrag;
  Vector2 endDrag;

  private int caseLength = 4;

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
      RaycastHit hit;
      bool physics = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out hit, 100.0f, LayerMask.GetMask("Board_Ecce"));
      if (physics)
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
          } else if(GetPiece(mouseOver) == null)
          {
            // There are no piece in the next move
            TryMovePiece(selectedPiece, mouseOver, startDrag);
          } else
          {
            // The piece is eatean
          }
        }
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

  private void TrySelectPiece(Vector2 position)
  {
     this.selectedPiece = this.GetPiece(position);
     this.startDrag = position;
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

  public void TryMovePiece(Piece p, Vector2 endDrag, Vector2 startDrag)
  {
    p.transform.position =
      (Vector3.right * ToBoardCoordinates(endDrag).x) +
      (Vector3.forward * ToBoardCoordinates(endDrag).y) +
      (Vector3.up * 2);
    pieces[
      (int)ToArrayCoordinates(startDrag).x,
      (int)ToArrayCoordinates(startDrag).y
      ] = null;
    pieces[
      (int) ToArrayCoordinates(endDrag).x,
      (int)ToArrayCoordinates(endDrag).y
      ] = p;
    selectedPiece = null;


    if (this.pieces[3, 2] != null)
    {
      Debug.Log(startDrag);
      Debug.Log("test" + this.pieces[3, 2].name);
    }
  }
  public void isFirstPieceMove() { }

  /*
   * Set of board of Entries Generator for both types.
   */
  private void GeneratePieces()
  {
    this.pieces[1, 1] = this.GeneratePiece(whitePiecePrefab,
      1 * caseLength + 2, 1 * caseLength + 2);
    this.pieces[6, 1] = this.GeneratePiece(blackPiecePrefab,
      6 * caseLength + 2, 1 * caseLength + 2);
  }

  /**
  * Single Piece Generator.
  */
  private Piece GeneratePiece(GameObject piecePrefab, int x, int y)
  {
    GameObject go = Instantiate(piecePrefab) as GameObject;
    go.AddComponent<Piece>();
    go.transform.SetParent(Board_Ecce.transform);
    Piece piece = go.GetComponent<Piece>();
    TryMovePiece(piece, new Vector2(x, y), new Vector2(x, y));
    return piece;
  }


  public void MovePiece(Piece p, int x1, int y1, int x2, int y2)
  {



  }


}