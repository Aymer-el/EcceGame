using UnityEngine;
using UnityEngine.UI;

public interface ISelect
{
  void TrySelectAPiece();
}

public class Global : MonoBehaviour
{
  /**** Dependency ****/
  // Board or awaiting pieces to enter in game or that.
  public GameObject whitePiecePrefab;
  public GameObject blackPiecePrefab;
  public GameObject whiteEccePrefab;
  public GameObject blackEccePrefab;
  public Text scoreWhite;
  public Text scoreBlack;
  public static Global EcceInstance;
  static bool isUIShown = false;
  public static bool IsUIShown { get => isUIShown; set => isUIShown = value; }

  /**** Relative to Game Object and View ****/
  // Unique set of Pieces.
  public Piece[,] pieces = new Piece[8, 6];
  public Piece[,] newPiecesNotOnBoard = new Piece[2, 8];
  public Piece[,] newEccePiece = new Piece[2, 8];
  // Use to determine winner
  public int countWhitePiecesOnBoard;
  public int countBlackPiecesOnBoard;

  /**** Action ****/
  public int player = 0;
  public bool isWhite;
  protected Vector2 mouseOver;
  protected Vector2 startDrag;

  int scoreWhiteInt = 0;
  int scoreBlackInt = 0;
  public static int WinnerInt { get; set; } = -1;

  public Client client;


    protected readonly int caseLength = 2;

  /**** View ****/
  protected Piece selectedPiece;

  /*
   * Gather all components.
   * Generate Pieces.
   */
  public void Awake()
  {
        EcceInstance = this;
        client = FindObjectOfType<Client>();
        if (client)
        {
            isWhite = !client.isHost;
        }
    scoreWhite = GameObject.Find("scoreWhite").GetComponent<Text>();
    scoreBlack = GameObject.Find("scoreBlack").GetComponent<Text>();
    this.GeneratePieces();
  }

  /*
   * Allow us to detect click, and drag and drop event.
   */
  private void Update()
  {
    this.UpdateMouseOver();
    scoreWhite.text = scoreWhiteInt.ToString();
    scoreBlack.text = scoreBlackInt.ToString();
    if(WinnerInt > -1)
    {

    }
  }

    public bool CanPlay()
    {
        return (isWhite && player == 0) || (!isWhite && player == 1);
    }

  /*
   * Once the user ask to move a Piece
   */
  private void UpdateMouseOver()
  {
    if (Camera.main && Input.GetMouseButtonDown(0) && !Global.IsUIShown && CanPlay())
    {
      bool physicsEcceBoard = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
         out RaycastHit hit, 25.0f, LayerMask.GetMask("EcceBoard"));
      bool physicsWhiteBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit1, 50f, LayerMask.GetMask("WhiteBanch"));
      bool physicsBlackBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit2, 50f, LayerMask.GetMask("BlackBanch"));
      if (physicsEcceBoard || physicsWhiteBanch || physicsBlackBanch)
      {
        // Saving mouseOver
        mouseOver.x = (int)hit.point.x;
        mouseOver.y = (int)hit.point.z;
        if (Input.GetMouseButtonDown(0))
        {
          if (selectedPiece == null)
          {
            // Selecting a new piece
            if (physicsWhiteBanch || physicsBlackBanch)
            {
              if(physicsWhiteBanch && player == 0)
              {
                TryPlaceNewPiece(player);
            
              } else if(physicsBlackBanch && player == 1)
              {
                TryPlaceNewPiece(player);
              }
                if (client)
                {
                    Debug.Log("isthereclient");
                    string msg = "CPLA|";
                    msg += (isWhite ? 0 : 1).ToString();
                    client.Send(msg);
                }
            } else
            // Selecting a piece
            {
              TrySelectPiece(mouseOver, player);
                if (client)
                    {
                        string msg = "CSEL|";
                        msg += (isWhite ? 0 : 1).ToString() + "|";
                        msg += mouseOver.x + "|";
                        msg += mouseOver.y;
                        client.Send(msg);
                    }
                }
          }
          else
          {
              //TryMovePiece(mouseOver, startDrag);
                if (client)
                {
                    string msg = "CMOV|";
                    msg += (isWhite ? 0 : 1).ToString() + "|";
                    msg += mouseOver.x + "|";
                    msg += mouseOver.y + "|";
                    msg += startDrag.x + "|";
                    msg += startDrag.y;
                    client.Send(msg);
                }
                    
            }
        }
      }
    }
  }

  protected Piece GetPiece(Vector2 position)
  {
    // Getting the piece out of the array
    Vector2 coordinates = ToArrayCoordinates(position);
    if(coordinates.x >= 0 && coordinates.x < 8 && coordinates.y >= 0 && coordinates.y < 6)
    {
      return pieces[
      (int)coordinates.x,
      (int)coordinates.y
      ];
    } else
    {
      return null;
    }
    
  }

  public virtual void TrySelectPiece(Vector2 mouseOver, int player)
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

  public virtual void TryMovePiece(Vector2 mouseOver, Vector2 startDrag)
  {
        Piece advPiece = GetPiece(mouseOver);
        if (this.selectedPiece != null && advPiece != null && !advPiece.name.Contains(this.selectedPiece.name.Substring(0, 5)))
        {
            // Eating piece
            if (GameLogic.IsMovePossible(true, selectedPiece.isEcce,
              ToBoardCoordinates(startDrag), ToBoardCoordinates(mouseOver)))
            {
                RemovingPiece(ToArrayCoordinates(mouseOver), true);
                Move(mouseOver, startDrag);
            }
        }
        else
        {
            // Moving piece
            // If there is no piece on the case && if it is a possible move
            if (GetPiece(mouseOver) == null && GameLogic.IsMovePossible(selectedPiece.isEcce, true,
              ToBoardCoordinates(startDrag), ToBoardCoordinates(mouseOver)))
            {
                Move(mouseOver, startDrag);
            }
            // Selecting another piece
            else
            {
                DeselectPiece(startDrag);
                TrySelectPiece(mouseOver, player);
            }
        }
  }

    public void Move(Vector2 mouseOver, Vector2 startDrag)
    {
        // Moving piece
        Piece p = selectedPiece;
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
        CheckPieceEvolution(mouseOver, player);
        CheckOneUp(p, ToArrayCoordinates(mouseOver), player);
        FinishTurn();
        DeselectPiece(mouseOver);
        DetermineWinner();
    }


  /*
   * Set of board of Entries Generator for both types.
   */
  protected void GeneratePieces()
  {
    for (var i = 0; i < 2; i++)
    {
      for (var j = 0; j < 7; j++)
      {
        Piece piece;
        Piece Ecce;
        if (i % 2 == 0)
        {
          piece = GeneratePiece(whitePiecePrefab,
            ToBoardCoordinates(new Vector2(-3, j * caseLength)));
         Ecce = GeneratePiece(whiteEccePrefab,
            ToBoardCoordinates(new Vector2(j * caseLength + 2, -10)));
        } else
        {
          piece = GeneratePiece(blackPiecePrefab,
            ToBoardCoordinates(new Vector2(18, j * caseLength)));
          Ecce = GeneratePiece(blackEccePrefab,
            ToBoardCoordinates(new Vector2(j * caseLength + 2, -12)));
        }
        newPiecesNotOnBoard[i, j] = piece;
        Ecce.isEcce = true;
        newEccePiece[i, j] = Ecce;
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
    go.transform.SetParent(this.transform);
    Piece piece = go.GetComponent<Piece>();
    piece.transform.position =
      (Vector3.right * coordinate.x) +
      (Vector3.forward * coordinate.y) +
      (Vector3.up * 1);
    return piece;
  }

  public void RemovingPiece(Vector2 mouseOver, bool countMinus)
  {
    Piece p = pieces[(int)mouseOver.x, (int)mouseOver.y];
    if (p.name.Contains("white"))
    {
      if(countMinus) countWhitePiecesOnBoard--;
    } else if (p.name.Contains("black"))
    {
      if(countMinus) countBlackPiecesOnBoard--;
    }
    p.gameObject.SetActive(false);
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

  public void DeselectPiece(Vector2 startDrag)
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
      countWhitePiecesOnBoard++;
      Move(
        new Vector2(1 * caseLength, 1 * caseLength),
        new Vector2(1 * caseLength, 1 * caseLength));
    }
    if (player == 1 && pieces[6, 1] == null)
    {
      selectedPiece = GetANewPiece(player);
      countBlackPiecesOnBoard++;
      Move(
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

  private Piece GetANewEccePiece(int player)
  {
    var i = 0;
    var found = false;
    while (i <= 7 && !found)
    {
      if (newEccePiece[player, i] != null)
      {
        found = true;
      }
      else
      {
        i++;
      }
    }
    Piece piece = newEccePiece[player, i];
    newEccePiece[player, i] = null;
    return piece;
  }

  public void CheckPieceEvolution(Vector2 mouseOver, int player)
  {
    Vector2 arrayCoordinates = ToArrayCoordinates(mouseOver);
    Vector2 boardCoordinates = ToBoardCoordinates(mouseOver);
    Piece piece = GetPiece(mouseOver);
    if (piece != null && !piece.isEcce)
    {
      if (arrayCoordinates.x == 1 && arrayCoordinates.y == 4
        && player == 0
        ||
         arrayCoordinates.x == 6 && arrayCoordinates.y == 4
         && player == 1
        )
      {
        Piece Ecce = GetANewEccePiece(player);
        RemovingPiece(arrayCoordinates, false);
        pieces[(int)arrayCoordinates.x, (int)arrayCoordinates.y] = Ecce;
        Ecce.transform.position =
        (Vector3.right * boardCoordinates.x) +
        (Vector3.forward * boardCoordinates.y) +
        (Vector3.up * 1);
      }
    }
  }

  public void CheckOneUp(Piece p, Vector2 mouseOver, int player)
  {
    if (mouseOver.x == 1 && mouseOver.y == 1 && p.name.Contains("black")
      ||
       mouseOver.x == 6 && mouseOver.y == 1 && p.name.Contains("white")
      )
    {
      if(player == 0)
      {
        scoreWhiteInt++;
      } else
      {
        scoreBlackInt++;
      }
      RemovingPiece(mouseOver, true);
    }
  }

  public bool IsPlayerPickingRightColorPiece(Piece piece, int player)
  {
    return piece.name.Contains("white") && player == 0
      ||
           piece.name.Contains("black") && player == 1;
  }

  public int NumberOfNewPieces(int player)
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

  public int NumberOfPiecesOnBoard(int player)
  {
    var i = 0;
    var found = false;
    while (i < 7 && !found)
    {
      if (pieces[player, i] != null)
      {
        found = true;
      }
      else
      {
        i++;
      }
    }
    return pieces.Length / 2 - (i + 1);
  }


  protected void DetermineWinner()
  {
    if (scoreWhiteInt >= NumberOfNewPieces(1) + countBlackPiecesOnBoard + scoreBlackInt) {
      Global.WinnerInt = 0;
    }
    if(scoreBlackInt >= NumberOfNewPieces(0) + countWhitePiecesOnBoard + scoreWhiteInt)
    {
      Global.WinnerInt = 1;
    }
  }

}