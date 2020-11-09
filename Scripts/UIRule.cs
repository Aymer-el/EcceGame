using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class UIRule : MonoBehaviour
{
  public bool isShowing = true;
  public GameObject CanvasMenu;
  public GameObject PanelMenu;
  public GameObject CanvasRulesStandardPiece;
  public GameObject CanvasRulesPalPiece;
  public GameObject CanvasRulesScore;
  public GameObject ButtonToggle;
  public GameObject ButtonMenuStandard;
  public GameObject ButtonMenuPal;
  public GameObject ButtonMenuScore;
  public GameObject ButtonMenuExit;

  public bool[] isShowingCanvas = new bool[3];

 

  public void Awake()
  {
    ButtonMenuStandard.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuStandartPiece"];
    ButtonMenuPal.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuPalPiece"];
    ButtonMenuExit.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuExit"];

    CanvasRulesStandardPiece.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["standardMove[1]"];
    CanvasRulesPalPiece.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["ecceMove[1]"];

    CanvasRulesScore.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["score[1]"];
   

    PlayStandardPiece(() => RefreshMenu(0));
    PlayerPalPiece(() => RefreshMenu(1));
    PlayerScore(() => RefreshMenu(2));
    NewGame(() => {
      Global.WinnerInt = -1;
      SceneManager.LoadScene("NewGameScene");
    });
    ToggleMenu(() => {
      isShowing = !isShowing;
      PanelMenu.SetActive(isShowing);
      Global.IsUIShown = isShowing;
    });
  }

  void PlayStandardPiece(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[0].onClick.AddListener(action);
  }

  void PlayerPalPiece(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[1].onClick.AddListener(action);
  }

  void PlayerScore(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[2].onClick.AddListener(action);
  }

  void NewGame(UnityAction action)
  {
    CanvasMenu.GetComponentsInChildren<Button>()[3].onClick.AddListener(action);
  }

  void ToggleMenu(UnityAction action)
  {
    ButtonToggle.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void RefreshMenu(int index)
  {
    for (var i = 0; i < isShowingCanvas.Length; i++)
    {
      if (i != index) isShowingCanvas[i] = false;
    }
    isShowingCanvas[index] = !isShowingCanvas[index];
  }

  // Update is called once per frame
  private void Update()
  {
    CanvasRulesStandardPiece.SetActive(isShowingCanvas[0]);
    CanvasRulesPalPiece.SetActive(isShowingCanvas[1]);
    CanvasRulesScore.SetActive(isShowingCanvas[2]);
  }

}
