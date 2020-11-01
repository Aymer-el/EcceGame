using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
  public GameObject menu;
  public static bool isShowing = true;
  public UnityEvent onClick;
  public GameObject ButtonShare;
  public GameObject ButtonPlay;
  public GameObject ButtonRules;
  public GameObject ButtonSound;
  public GameObject ButtonCredits;
  // Start is called before the first frame update


  public void Awake()
  {
    menu.SetActive(isShowing);
    NewGame(() => SceneManager.LoadScene("NewGameScene"));
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
    }
}
