using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button attackButton;
    public Button endTurnButton;
    public Button abilitiesButton;
    public TMPro.TextMeshProUGUI manaText;
    public UnityEngine.UI.Image manaBar_Fill;
    public static UIManager Instance;


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

        manaText.text = "Mana: " + BattleManager.Instance.player.currentMana;


        float fill = (float)BattleManager.Instance.player.currentMana /
             BattleManager.Instance.player.maxMana;

        manaBar_Fill.fillAmount = fill;
        Debug.Log("Fill: " + fill);
    }

    public void RefreshUI()
    {
        var player = BattleManager.Instance.player;

        manaText.text = "Mana: " + player.currentMana;

        float fill = (float)player.currentMana / player.maxMana;
        manaBar_Fill.fillAmount = fill;
    }



}
