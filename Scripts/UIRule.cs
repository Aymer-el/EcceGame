using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIRule : MonoBehaviour
{
    public static bool areGuidelinesOn = false;
    public GameObject CanvasMenu;
    public GameObject PanelGuidelinesEcce;
    public static bool showPanelGuidelinesEcce = false;
    public static bool isEcceUnderstood = false;
    public GameObject ButtonUnderstoodEcce;
    public GameObject PanelGuidelinesScore;
    public static bool showPanelGuidelinesScore = false;
    public static bool isScoreUnderstood = false;
    public GameObject ButtonUnderstoodScore;
    public GameObject PanelGuidelinesBanch;
    public static bool showPanelGuidelinesBanch = true;
    public static bool isBanchUnderstood = false;
    public GameObject ButtonUnderstoodBanch;
    public GameObject ButtonToggleGuidelines;
    public static int numberOfunderstoodRules = 0;

    public void Awake()
    {
        PanelGuidelinesBanch.GetComponentInChildren<TMP_Text>().text =
            I18n.Fields["standardMove[1]"];
        PanelGuidelinesEcce.GetComponentInChildren<TMP_Text>().text =
            I18n.Fields["ecceMove[1]"];
        PanelGuidelinesScore.GetComponentInChildren<TMP_Text>().text =
            I18n.Fields["score[1]"];

        ToggleGuidelines(() => {
            areGuidelinesOn = !areGuidelinesOn;
            if (!areGuidelinesOn)
            {
                ButtonToggleGuidelines.GetComponent<Image>().color = new Color32(87, 153, 99, 255);
                isEcceUnderstood = false;
                isScoreUnderstood = false;
                isBanchUnderstood = false;
                showPanelGuidelinesEcce = false;
                showPanelGuidelinesScore = false;
                showPanelGuidelinesBanch = true;
                numberOfunderstoodRules = 0;
            } else
            {
                ButtonToggleGuidelines.GetComponent<Image>().color = new Color32(27, 183, 46, 255);
            }
        });
        UnderstoodGuidelinesEcce(() => {
            isEcceUnderstood = !isEcceUnderstood;
            numberOfunderstoodRules++;
        });
        UnderstoodGuidelinesScore(() => {
            isScoreUnderstood = !isScoreUnderstood;
            numberOfunderstoodRules++;
        });
        UnderstoodGuidelinesBanch(() => {
            isBanchUnderstood = !isBanchUnderstood;
            numberOfunderstoodRules++;
        });
    }

    void ToggleGuidelines(UnityAction action)
    {
    ButtonToggleGuidelines.GetComponentInChildren<Button>().onClick.AddListener(action);
    } 

    void UnderstoodGuidelinesEcce(UnityAction action)
    {
        ButtonUnderstoodEcce.GetComponent<Button>().onClick.AddListener(action);
    }

    void UnderstoodGuidelinesBanch(UnityAction action)
    {
        ButtonUnderstoodBanch.GetComponent<Button>().onClick.AddListener(action);
    }

    void UnderstoodGuidelinesScore(UnityAction action)
    {
        ButtonUnderstoodScore.GetComponent<Button>().onClick.AddListener(action);
    }


    // Update is called once per frame
    private void Update()
    {
        Vector2 mouseOver;
        bool physicsEcceBoard = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
        out RaycastHit hit, 25.0f, LayerMask.GetMask("EcceBoard"));
        if (physicsEcceBoard)
        {
            mouseOver.x = (int)hit.point.x;
            mouseOver.y = (int)hit.point.z;
            Vector2 boardCoordinates = Scenario.EcceInstance.ToArrayCoordinates(mouseOver);
            if (Camera.main && Input.GetMouseButtonDown(0) && areGuidelinesOn)
            {
                if ((boardCoordinates.y == 4 && boardCoordinates.x == 1) || (boardCoordinates.y == 4 && boardCoordinates.x == 6))
                {
                    showPanelGuidelinesEcce = true;
                }
                if (boardCoordinates.y == 1 && boardCoordinates.x == 1 || boardCoordinates.y == 1 && boardCoordinates.x == 6)
                {
                    UIRule.showPanelGuidelinesScore = true;
                }
            }
        }
        if (areGuidelinesOn)
        {
            PanelGuidelinesBanch.SetActive(showPanelGuidelinesBanch && !isBanchUnderstood);
            PanelGuidelinesEcce.SetActive(showPanelGuidelinesEcce && !isEcceUnderstood);
            PanelGuidelinesScore.SetActive(showPanelGuidelinesScore && !isScoreUnderstood);
        }
        ButtonToggleGuidelines.GetComponentInChildren<TMP_Text>().text = "Guidelines" + "\n" + numberOfunderstoodRules + "/3";
    }
}
