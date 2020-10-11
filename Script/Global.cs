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
  public GameObject GO_BoardBlack;
  public GameObject GO_BoardWhite;
  private Board_IO BoardWhite;
  private Board_IO BoardBlack;
  // Board du DamiersEcce.
  private Board_Ecce Board_Ecce;
  private Board_Ecce Board_EcceLayerCastDetection;
  /**** Action ****/
  Vector2 actionVector;
  Vector2 startDrag;
  Vector2 endDrag;

  private int caseLength = 4;

  /**** View ****/
  private Piece selectedPiece;
  private bool doShowMoves = false;


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
   * During Drag and Drop and after a click is done.
   */
  private void UpdateMouseOver()
  {
    if (Camera.main && Input.GetMouseButtonDown(0))
    {
      RaycastHit hit;
      bool physics = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out hit, 100.0f, LayerMask.GetMask("Board_Ecce"));
      if (physics)
      {
        actionVector.x = (int)hit.point.x;
        actionVector.y = (int)hit.point.z;
        if(Input.GetMouseButtonDown(0))
          this.selectPiece((int)actionVector.x, (int)actionVector.y);
      }
    }
  }

  private void selectPiece(int x, int y)
  {
    if (x < 0 || x >= pieces.Length * caseLength
      && y < 0 || y >= pieces.Length * caseLength)
      return;

    Piece p = pieces[Mathf.FloorToInt(x / caseLength),
      Mathf.FloorToInt(y / caseLength)];
    Debug.Log(Mathf.FloorToInt(y / caseLength));

    if(p != null)
    {
      selectedPiece = p;
      startDrag = actionVector;
      Debug.Log("selected Piece", selectedPiece);
    }
  }


  public void tryMove(int x1, int y1, int x2, int y2)
  {
    startDrag = new Vector2(x1, y1);
    endDrag = new Vector2(x2, y2);
  }


  /**
   * Capting LayerMask returning value to get corresponding component
   */
  private T LayerRayCastDetection<T>(string mask, T referent) where T : MonoBehaviour
  {
    RaycastHit hit;
    bool physics = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
       out hit, 100.0f, LayerMask.GetMask(mask));
    if (physics)
    {
      actionVector.x = (int)hit.point.x;
      actionVector.y = (int)hit.point.z;
      return hit.transform.gameObject.GetComponent<T>();
    }
    else
    {
      return referent;
    }
  }

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
    MovePiece(piece, x, y);
    return piece;
  }


  public void MovePiece(Piece p, int x, int y)
  {
    p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + (Vector3.up * 2);
  }

  /*
   * Set of board of Entries Generator for both types.
   */
  private void GenerateBoardsIO()
  {
    BoardBlack = GenerateBoardIO(GO_BoardBlack).Build_Board_IO(new Color(ColorEnum.Black));
    BoardWhite = GenerateBoardIO(GO_BoardWhite).Build_Board_IO(new Color(ColorEnum.White));
  }

  /**
  * Single Piece Generator.
  */
  private Board_IO GenerateBoardIO(GameObject boardPrefab)
  {
    GameObject go = Instantiate(boardPrefab) as GameObject;
    return go.AddComponent<Board_IO>();
  }


  /**
   * Detects context of a player action.
   */
  public Board CluesOrAction()
  {
    // Action of placing a piece on the board.
    if (!doShowMoves)
    {
      this.ShowMoves(selectedPiece);
      Debug.Log(this.selectedPiece);
      doShowMoves = selectedPiece != null;
      // Board_EcceLayerCastDetection = null;
      actionVector = new Vector3();
    }
    else if (selectedPiece != null && Board_EcceLayerCastDetection != null)
    {
      this.selectedPiece.transform.SetParent(Board_Ecce.transform);
      this.selectedPiece.transform.position = Board_Ecce.transform.position;
      MovePiece(selectedPiece, 1, 1);
      doShowMoves = false;
      selectedPiece = null;
      Board_EcceLayerCastDetection = null;
    }
    return Board_Ecce;
  }


  public Piece PieceAction(Piece piece, Input input)
  {
    //if (piece.Square.Board == Board_EntryBoard) {
    if (false)
    {
      /*
      if(Input.GetMouseButtonUp(0)) {
          this.ShowMoves(piece);
      }
      return piece;
      */
    }
    else
    {
      return piece;
    }
  }


  /**
   * Return Squares available to players.
   */
  public Square[,] ShowMoves(Piece piece)
  {
    return new Square[1, 1];
  }
}