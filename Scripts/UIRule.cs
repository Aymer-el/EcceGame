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
  public GameObject PanelRulesPawns;
  public GameObject PanelRulesEcces;
  public GameObject PanelRulesScore;
  public Button ButtonToggle;
  public GameObject ButtonMenuStandard;
  public GameObject ButtonMenuPal;
  public GameObject ButtonMenuScore;
  public bool IsUiOn = true;

  public bool[] isShowingCanvas = new bool[3];

 

  public void Awake()
  {
    ButtonMenuStandard.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuStandartPiece"];
    ButtonMenuPal.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuPalPiece"];
  
    PanelRulesPawns.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["standardMove[1]"];
    PanelRulesEcces.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["ecceMove[1]"];

    PanelRulesScore.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["score[1]"];
   

    PlayStandardPiece(() => RefreshMenu(0));
    PlayerPalPiece(() => RefreshMenu(1));
    PlayerScore(() => RefreshMenu(2));
    ToggleMenu(() => {
      isShowing = !isShowing;
      if (!isShowing)
      {
        ButtonToggle.GetComponent<Image>().color = new Color32(87, 153, 99, 255);
      } else
      {
        ButtonToggle.GetComponent<Image>().color = new Color32(27, 183, 46, 255);
      }
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
    PanelRulesPawns.SetActive(isShowingCanvas[0]);
    PanelRulesEcces.SetActive(isShowingCanvas[1]);
    PanelRulesScore.SetActive(isShowingCanvas[2]);
  }

}
