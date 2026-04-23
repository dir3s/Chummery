using UnityEngine;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    public BattleManager battle;

    public CanvasGroup abilitiesGroup;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject abilitiesPanel;


    public Ability[] abilities;

    private void Start()
    {
        DisableAbilities();
    }

    public void EnableAbilities()
    {
        abilitiesPanel.SetActive(true);
        abilitiesGroup.interactable = true;
        abilitiesGroup.blocksRaycasts = true;
        abilitiesGroup.alpha = 1f;
    }

    public void DisableAbilities()
    {

        abilitiesGroup.interactable = false;
        abilitiesGroup.blocksRaycasts = false;
        abilitiesGroup.alpha = 0.3f;
        abilitiesPanel.SetActive(false);
    }

   

    public void ShowAbilityInfo(int index)
    {
        nameText.text = abilities[index].name;
        descriptionText.text = abilities[index].description;
    }

   

    public void UseAbility(int index)
    {
        battle.enemy.TakeDamage(abilities[index].damage);

        DisableAbilities();
        battle.EndPlayerTurn();
    }
}