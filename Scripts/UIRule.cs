using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIRule : MonoBehaviour
{
  public GameObject CanvasMenu;
  public GameObject CanvasRulesStandardPiece;
  public GameObject CanvasRulesPalPiece;
  public GameObject CanvasRulesScore;

  public bool[] isShowingCanvas = new bool[3];

  
  public void Start()
  {
  }

  public void Awake()
  {
    CanvasRulesStandardPiece.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["standardMove[1]"] + '\n' +
    I18n.Fields["standardMove[2]"] + '\n' + I18n.Fields["standardMove[3]"]
    + '\n' + I18n.Fields["standardMove[4]"];

    CanvasRulesPalPiece.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["palMove[1]"] + '\n' +
    I18n.Fields["palMove[2]"] + '\n' + I18n.Fields["palMove[3]"];


    CanvasRulesScore.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["score[1]"] + '\n' + I18n.Fields["score[2]"];

    PlayStandardPiece(() => RefreshMenu(0));
    PlayerPalPiece(() => RefreshMenu(1));
    PlayerScore(() => RefreshMenu(2));
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

  void RefreshMenu(int index)
  {
    for (var i = 0; i < isShowingCanvas.Length; i++)
    {
      if (i != index) isShowingCanvas[i] = false;
    }
    isShowingCanvas[index] = !isShowingCanvas[index];
  }


  private void Update()
  {
    CanvasRulesStandardPiece.SetActive(isShowingCanvas[0]);
    CanvasRulesPalPiece.SetActive(isShowingCanvas[1]);
    CanvasRulesScore.SetActive(isShowingCanvas[2]);
  }

}
