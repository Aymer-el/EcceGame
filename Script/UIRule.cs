using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIRule : MonoBehaviour
{
  public GameObject CanvasMenu;
  public GameObject CanvasRulesStandardPiece;
  public GameObject CanvasRulesPalPiece;
  public GameObject CanvasRulesCase;
  public bool[] isShowingCanvas = new bool[3];


  public void Awake()
  {
    PlayStandardPiece(() => RefreshMenu(0));
    PlayerPalPiece(() => RefreshMenu(1));
    PlayCasePiece(() => RefreshMenu(2));
  }

  void PlayStandardPiece(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(action);
  }

  void PlayerPalPiece(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[1].onClick.AddListener(action);
  }

  void PlayCasePiece(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[2].onClick.AddListener(action);
  }

  void RefreshMenu(int index)
  {
    for (var i = 0; i < isShowingCanvas.Length; i++)
    {
      isShowingCanvas[i] = false;
    }
    isShowingCanvas[index] = !isShowingCanvas[index];
  }


  private void Update()
  {
    
    CanvasRulesStandardPiece.SetActive(isShowingCanvas[0]);
    CanvasRulesPalPiece.SetActive(isShowingCanvas[1]);
    CanvasRulesCase.SetActive(isShowingCanvas[2]);
  }




}
