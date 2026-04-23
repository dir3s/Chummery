using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button attackButton;
    public Button endTurnButton;

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
    }


}
