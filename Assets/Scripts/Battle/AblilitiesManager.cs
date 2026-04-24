using UnityEngine;
using TMPro;

public class AbilityManager : MonoBehaviour
{
    public BattleManager battle;

    public CanvasGroup abilitiesGroup;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject abilitiesPanel;
    private bool isOpen = false;

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
        Ability ability = abilities[index];

        if (battle.player.currentMana < ability.manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        battle.player.currentMana -= ability.manaCost;
        UIManager.Instance.RefreshUI();
        battle.enemy.TakeDamage(ability.damage);

        Debug.Log("Used ability: " + ability.name +
                  " | Mana left: " + battle.player.currentMana);

        DisableAbilities();
        battle.EndPlayerTurn();
    }

    public void HighlightButton(GameObject button)
    {
        button.transform.localScale = Vector3.one * 1.1f;
    }

    public void UnhighlightButton(GameObject button)
    {
        button.transform.localScale = Vector3.one;
    }


    public void ToggleAbilities()
    {
        if (isOpen)
        {
            CloseAbilities();
        }
        else
        {
            OpenAbilities();
        }
    }

    public void OpenAbilities()
    {
        abilitiesPanel.SetActive(true);
        abilitiesGroup.interactable = true;
        abilitiesGroup.blocksRaycasts = true;
        abilitiesGroup.alpha = 1f;
        isOpen = true;
    }

    public void CloseAbilities()
    {
        abilitiesPanel.SetActive(false);
        abilitiesGroup.interactable = false;
        abilitiesGroup.blocksRaycasts = false;
        abilitiesGroup.alpha = 0.3f;
        isOpen = false;
    }
}