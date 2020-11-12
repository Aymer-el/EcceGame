using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
  public GameObject menu;
  public static bool isShowing = true;
  public GameObject ButtonToggle;
  public GameObject ButtonShare;
  public GameObject ButtonNewGame;
  public GameObject ButtonRules;
  public GameObject ButtonSound;
  public GameObject ButtonCredits;
  public GameObject PanelWinner;
  // Start is called before the first frame update


  public void Awake()
  {
    menu.SetActive(isShowing);
    PanelWinner.SetActive(false);
    NewGame(() => {
      Global.WinnerInt = -1;
      SceneManager.LoadScene("NewGameScene");
    });
    Rules(() => SceneManager.LoadScene("RulesScene"));
    Sound(() => GetComponent<AudioSource>().mute = !GetComponent<AudioSource>().mute);
    ToggleMenu(() => {
      isShowing = !isShowing;
      if (!isShowing)
      {
        ButtonToggle.GetComponent<Image>().color = new Color32(87, 153, 99, 255);
      }
      else
      {
        ButtonToggle.GetComponent<Image>().color = new Color32(27, 183, 46, 255);
      }
      Global.IsUIShown = isShowing;
    });
    ButtonShare.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuShare"];
    ButtonNewGame.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuNewGame"];
    ButtonRules.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuRules"];
    ButtonSound.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuSound"];
    ButtonCredits.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuCredits"];
    ButtonToggle.GetComponentInChildren<TMP_Text>().text =
      I18n.Fields["menuToggle"];
  }

  void NewGame(UnityAction action)
  {
    ButtonNewGame.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void Rules(UnityAction action)
  {
    ButtonRules.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void Sound(UnityAction action)
  {
    ButtonSound.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void ToggleMenu(UnityAction action)
  {
    ButtonToggle.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  // Update is called once per frame
  private void Update()
    {
      if (Global.WinnerInt > -1)
      {
        PanelWinner.SetActive(true);
        PanelWinner.GetComponentInChildren<TMP_Text>().text =
        I18n.Fields["winner[" + Global.WinnerInt + "]"] + I18n.Fields["winner[2]"];
      }
     menu.SetActive(isShowing);
  }
}
