using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button attackButton;
    public Button endTurnButton;
    public Button abilitiesButton;
    public TMPro.TextMeshProUGUI manaText;
    public UnityEngine.UI.Image manaBar_Fill;
    public TMPro.TextMeshProUGUI manaText2;
    public UnityEngine.UI.Image manaBar_Fill2;
    public static UIManager Instance;
    public GameObject manaUI;
    public GameObject attackButtonObj;
    public GameObject endTurnButtonObj;
    public GameObject abilitiesButtonObj;
    public GameObject abilitiesPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        attackButton.onClick.AddListener(OnAttackClicked);
        endTurnButton.onClick.AddListener(OnEndTurnClicked);
    }

    void OnAttackClicked()
    {
        Debug.Log("Attack clicked!");

        BattleManager.Instance.PlayerAttack(); 
    }

    void OnEndTurnClicked()
    {
        BattleManager.Instance.EndPlayerTurn();
    }

    private void Update()
    {
        bool isPlayerTurn = BattleManager.Instance.playerTurn;

        attackButton.interactable = isPlayerTurn;
        endTurnButton.interactable = isPlayerTurn;
        abilitiesButton.interactable = isPlayerTurn;

        manaText.text = ""+BattleManager.Instance.player.currentMana;

        manaText2.text = "" + BattleManager.Instance.player.currentMana;


        float fill = (float)BattleManager.Instance.player.currentMana /
             BattleManager.Instance.player.maxMana;

        manaBar_Fill.fillAmount = fill;
        manaBar_Fill2.fillAmount = fill;

    }

    public void RefreshUI()
    {
        var player = BattleManager.Instance.player;

        manaText.text = "" + player.currentMana;

        float fill = (float)player.currentMana / player.maxMana;
        manaBar_Fill.fillAmount = fill;
        manaBar_Fill2.fillAmount = fill;

    }
    public void HideBattleUI()
    {
        attackButtonObj.SetActive(false);
        endTurnButtonObj.SetActive(false);
        abilitiesButtonObj.SetActive(false);
        abilitiesPanel.SetActive(false);
        manaUI.SetActive(false);
    }

    public void ShowBattleUI()
    {
        attackButtonObj.SetActive(true);
        endTurnButtonObj.SetActive(true);
        abilitiesButtonObj.SetActive(true);

        manaUI.SetActive(true);
    } 
}
