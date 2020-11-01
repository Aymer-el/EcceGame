using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIRule : MonoBehaviour
{
  public GameObject CanvasMenu;
  public GameObject CanvasRulesStandardPiece;
  public GameObject CanvasRulesPalPiece;
  public static bool isShowing;

  public void Awake()
  {
    PlayRules(() => isShowing = !isShowing);
  }

  void PlayRules(UnityAction action)
  {
    CanvasMenu.GetComponentInChildren<Button>().onClick.AddListener(action);
    GameObject.Find("ButtonStandardPiece").GetComponent<Text>();
  }

  private void Update()
  {
  }


}
