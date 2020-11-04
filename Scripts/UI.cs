using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI : MonoBehaviour
{
  public GameObject menu;
  public static bool isShowing = true;
  public GameObject ButtonShare;
  public GameObject ButtonPlay;
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
  }

  void NewGame(UnityAction action)
  {
    ButtonPlay.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void Rules(UnityAction action)
  {
    ButtonRules.GetComponentInChildren<Button>().onClick.AddListener(action);
  }

  void Sound(UnityAction action)
  {
    ButtonSound.GetComponentInChildren<Button>().onClick.AddListener(action);
  }
  // Update is called once per frame
  private void Update()
    {
      if (Input.GetKeyDown("escape"))
      {
        isShowing = !isShowing;
        menu.SetActive(isShowing);
      }
      if (Global.WinnerInt > -1)
      {
        PanelWinner.SetActive(true);
        PanelWinner.GetComponentInChildren<TMP_Text>().text =
        I18n.Fields["winner[" + Global.WinnerInt + "]"] + I18n.Fields["winner[2]"];
      }
  }
}
